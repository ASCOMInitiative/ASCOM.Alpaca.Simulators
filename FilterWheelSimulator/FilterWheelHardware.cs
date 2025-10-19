namespace ASCOM.Simulators
{
    using System;
    using System.Drawing;
    using System.Timers;

    using ASCOM.Common;
    using ASCOM.Common.Interfaces;
    using OmniSim.BaseDriver;

    /// <summary>
    /// The FilterWheel hardware simulation class.
    /// </summary>
    public class FilterWheelHardware
    {
        // Used to track id registry entries exist or need updating
        private const string RegVersion = "1";

        // How often we pump the hardware
        private readonly int timerTickInterval = 100;

        // Create some 'realistic' defaults
        private readonly Color[] defaultColors = [Color.Red, Color.Green, Color.Blue, Color.Gray,
                                                Color.DarkRed, Color.Teal, Color.Violet, Color.Black];

        // Sync object
        private readonly object objSync = new();


        private readonly IProfile settingsProfile;

        private readonly Timer timer = new(100)
        {
            AutoReset = true,
        };

        // Time required to complete the current Move
        private int timeToMove;

        // Keeps track of the elapsed time of the current Move
        private int timeElapsed;

        // Current filter position
        private short position;

        // Filter position we want to get to
        private short targetPosition;

        // FilterWheel in motion?
        private bool moving;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterWheelHardware"/> class.
        /// This is not safe to call directly, it is used to auto-generate the Settings APIs.
        /// </summary>
        public FilterWheelHardware()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterWheelHardware"/> class.
        /// </summary>
        /// <param name="logger">An ASCOM Logger for reports.</param>
        /// <param name="profile">An ASCOM Profile to store settings.</param>
        public FilterWheelHardware(ILogger logger, IProfile profile)
        {
            this.Logger = logger;
            this.settingsProfile = profile;

            this.moving = false;

            this.AllFilterNames = new string[8];
            this.AllFocusOffsets = new int[8];
            this.AllFilterColours = new Color[8];

            this.timer.Elapsed += this.OnTimedEvent;
            this.timer.Start();

            this.Initialize();
        }

        /// <summary>
        /// Gets the setting indicating the time to move between filter positions (milliseconds).
        /// </summary>
        public Setting<int> FilterChangeTimeInterval { get; } = new Setting<int>("FilterChangeTimeInterval", "Time to move between filter positions (milliseconds)", 1000);

        /// <summary>
        /// Gets the setting indicating if the driver can implement names.
        /// </summary>
        public Setting<bool> ImplementsNames { get; } = new Setting<bool>("ImplementsNames", "True if the driver implements names", true);

        /// <summary>
        /// Gets the setting indicating if the driver can implement focus offsets.
        /// </summary>
        public Setting<bool> ImplementsOffsets { get; } = new Setting<bool>("ImplementsOffsets", "True if the driver implements offsets", true);

        /// <summary>
        /// Gets the setting indicating if the driver can interrupt moves.
        /// </summary>
        public Setting<bool> PreemptMoves { get; } = new Setting<bool>("PreemptMoves", "True if the driver can interrupt moves", false);

        /// <summary>
        /// Gets the setting reporting the number of slots.
        /// </summary>
        public Setting<int> Slots { get; } = new Setting<int>("Slots", "Number of filter wheel positions, 1-8", 6);

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersion { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-3", 3);

        /// <summary>
        /// Gets the delay for the connect timer
        /// </summary>
        public Setting<short> ConnectDelay { get; } = new Setting<short>("ConnectDelay", "The delay to be used for Connect() in milliseconds, allowed values are 1-30000", 1500);

        /// <summary>
        /// Gets or sets the Array of filter name strings.
        /// </summary>
        public string[] AllFilterNames { get; set; }

        /// <summary>
        /// Gets or sets the Array of focus offsets.
        /// </summary>
        public int[] AllFocusOffsets { get; set; }

        /// <summary>
        /// Gets the Array of filter colors.
        /// </summary>
        public Color[] AllFilterColours { get; }

        /// <summary>
        /// Gets the filter names for the current number of slots.
        /// </summary>
        public string[] FilterNames
        {
            get
            {
                string[] temp = this.AllFilterNames;
                Array.Resize(ref temp, this.Slots.Value);
                return temp;
            }
        }

        /// <summary>
        /// Gets the filter offsets for the current number of slots.
        /// </summary>
        public int[] FocusOffsets
        {
            get
            {
                int[] temp = this.AllFocusOffsets;
                Array.Resize(ref temp, this.Slots.Value);
                return temp;
            }
        }

        /// <summary>
        /// Gets or sets the filter wheel position.
        /// </summary>
        public short Position
        {
            get
            {
                lock (this.objSync)
                {
                    if (this.moving)
                    {
                        // Spec. says we must return -1 if position not determined or moving
                        return -1;
                    }
                    else
                    {
                        // Otherwise return position
                        return this.position;
                    }
                }
            }

            set
            {
                lock (this.objSync)
                {
                    this.Logger.LogInformation("FilterWheel - Set Position to " + value);

                    // position range check
                    if (value >= this.Slots.Value || value < 0)
                    {
                        throw new ASCOM.InvalidValueException($"Position: {value} is out of range 0 - {this.Slots.Value}");
                    }

                    this.targetPosition = value;

                    // check if we are already there!
                    if (value == this.position)
                    {
                        this.Logger.LogInformation($"FilterWheel - Set Position, Already at position {this.position} no move required)");
                        return;
                    }

                    // check if we are already moving
                    if (this.moving)
                    {
                        if (this.PreemptMoves.Value)
                        {
                            // Stop the motor
                            this.AbortMove();
                        }
                        else
                        {
                            throw new InvalidOperationException("The FilterWheel is already moving");
                        }
                    }

                    // Find the shortest distance between two filter positions
                    int jumps = Math.Min(Math.Abs(this.position - this.targetPosition), this.Slots.Value - Math.Abs(this.position - this.targetPosition));
                    this.timeToMove = jumps * this.FilterChangeTimeInterval.Value;

                    // trigger the "motor"
                    this.moving = true;

                    // log action
                    this.Logger.LogVerbose($"FilterWheel - Set position to {this.targetPosition} in progress");
                }
            }
        }

        /// <summary>
        /// Gets the Logger.
        /// </summary>
        internal ILogger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Save the current settings.
        /// </summary>
        public void SaveSettings()
        {
            this.settingsProfile.SetSetting(this.Slots);
            this.settingsProfile.SetSetting(this.FilterChangeTimeInterval);
            for (int i = 0; i <= 7; i++)
            {
                this.settingsProfile.WriteValue($"FilterNames {i}", this.AllFilterNames[i]);
                this.settingsProfile.WriteValue($"FocusOffsets {i}", this.AllFocusOffsets[i].ToString());
                this.settingsProfile.WriteValue($"Filter {i} Color", this.AllFilterColours[i].Name);
            }

            this.settingsProfile.SetSetting(this.ImplementsNames);
            this.settingsProfile.SetSetting(this.ImplementsOffsets);
            this.settingsProfile.SetSetting(this.PreemptMoves);
            this.settingsProfile.SetSetting(this.InterfaceVersion);

            this.LoadSettings();
        }

        /// <summary>
        /// Initialize/finalize for server startup/shutdown.
        /// </summary>
        private void Initialize()
        {
            this.LoadSettings();
        }

        private void AbortMove()
        {
            this.Logger.LogInformation("FilterWheel - Abort move");

            // Clear the elapsed time
            this.timeElapsed = 0;

            // Stop moving
            this.moving = false;

            // Set the position intermediate between start and end
            this.position = (short)Math.Floor(Math.Abs(this.targetPosition - this.position) / 2.0);
            this.Logger.LogInformation("FilterWheel - Abort done");
        }

        private void UpdateState()
        {
            lock (this.objSync)
            {
                if (this.moving)
                {
                    // We are moving so increment the elapsed move time
                    this.timeElapsed += this.timerTickInterval;

                    // Have we reached the filter position yet?
                    if (this.timeElapsed >= this.timeToMove)
                    {
                        // Clear the elapsed time
                        this.timeElapsed = 0;

                        // Stop moving
                        this.moving = false;

                        // Set the new position
                        this.position = this.targetPosition;
                    }
                }
            }
        }

        internal void LoadSettings()
        {
            try
            {
                if (this.settingsProfile.GetValue("RegVer", string.Empty) != RegVersion)
                {
                    this.SetDefaultSettings();
                }

                // Read the hardware & driver config
                this.Slots.Value = this.settingsProfile.GetSettingReturningDefault(this.Slots);
                if (this.Slots.Value < 1 || this.Slots.Value > 8)
                {
                    this.Slots.Value = 6;
                }

                this.position = 0;
                this.FilterChangeTimeInterval.Value = Convert.ToInt32(this.settingsProfile.GetSettingReturningDefault(this.FilterChangeTimeInterval));
                this.InterfaceVersion.Value = Convert.ToInt16(this.settingsProfile.GetSettingReturningDefault(this.InterfaceVersion));
                this.ImplementsNames.Value = Convert.ToBoolean(this.settingsProfile.GetSettingReturningDefault(this.ImplementsNames));
                this.ImplementsOffsets.Value = Convert.ToBoolean(this.settingsProfile.GetSettingReturningDefault(this.ImplementsOffsets));
                this.PreemptMoves.Value = Convert.ToBoolean(this.settingsProfile.GetSettingReturningDefault(this.PreemptMoves));
                for (int i = 0; i <= 7; i++)
                {
                    this.AllFilterNames[i] = this.settingsProfile.GetValue($"FilterNames {i}");
                    this.AllFocusOffsets[i] = Convert.ToInt32(this.settingsProfile.GetValue($"FocusOffsets {i}"));
                    this.AllFilterColours[i] = Color.FromName(this.settingsProfile.GetValue($"Filter {i} Color", this.defaultColors[i].Name));
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"FilterWheel - Failed to load settings with error: {ex.Message}");
                this.SetDefaultSettings();
            }
        }

        private void SetDefaultSettings()
        {
            string[] names = ["Red", "Green", "Blue", "Clear", "Ha", "OIII", "LPR", "Dark"];
            Random rand = new();

            this.settingsProfile.WriteValue("RegVer", RegVersion);
            this.settingsProfile.WriteValue("Position", "0");
            this.settingsProfile.SetSettingDefault(this.Slots);
            this.settingsProfile.SetSettingDefault(this.FilterChangeTimeInterval);
            this.settingsProfile.SetSettingDefault(this.ImplementsNames);
            this.settingsProfile.SetSettingDefault(this.ImplementsNames);
            this.settingsProfile.SetSettingDefault(this.ImplementsOffsets);
            this.settingsProfile.SetSettingDefault(this.PreemptMoves);
            this.settingsProfile.SetSettingDefault(this.InterfaceVersion);
            for (int i = 0; i < 8; i++)
            {
                this.settingsProfile.WriteValue($"FilterNames {i}", names[i]);
                this.settingsProfile.WriteValue($"FocusOffsets {i}", rand.Next(10000).ToString());
                this.settingsProfile.WriteValue($"Filter {i} Color", this.defaultColors[i].Name);
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.UpdateState();
        }
    }
}