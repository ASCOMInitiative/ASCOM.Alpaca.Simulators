// Ignore Spelling: ASCOM

using ASCOM.Common;
using ASCOM.Common.Interfaces;
using OmniSim.BaseDriver;
using System;
using System.Drawing;
using System.Timers;

namespace ASCOM.Simulators
{
    //
    // Implements the simulated hardware
    //
    public class FilterWheelHardware
    {
        private int timeToMove;               // Time required to complete the current Move
        private int timeElapsed;              // Keeps track of the elapsed time of the current Move
        private int timerTickInterval = 100;  // How often we pump the hardware

        // Current filter position
        private short position;

        // invoking the full method
        private short targetPosition;         // Filter position we want to get to

        private bool moving;                  // FilterWheel in motion?

        private const string m_sRegVer = "1";             // Used to track id registry entries exist or need updating

        //
        // Create some 'realistic' defaults
        //
        private Color[] DefaultColors = new Color[8] {Color.Red, Color.Green, Color.Blue, Color.Gray,
                                                Color.DarkRed, Color.Teal, Color.Violet, Color.Black};

        //
        // Sync object
        //
        private object s_objSync = new object();	// Better than lock(this) - Jeffrey Richter, MSDN Jan 2003

        private IProfile Profile;

        private int SCODE_VAL_OUTOFRANGE = ErrorCodes.InvalidValue;
        private const string MSG_VAL_OUTOFRANGE = "The value is out of range";

        private int SCODE_MOVING = ErrorCodes.DriverBase + 0x405;
        private const string MSG_MOVING = "The filterwheel is already moving";


        private Timer timer = new Timer(100)
        {
            AutoReset = true,
        };

        /// <summary>
        /// This is not safe to call, it is use to create a default class used to create dynamic settings
        /// </summary>
        public FilterWheelHardware()
        {

        }

        //
        // Constructor - initialize state
        //
        public FilterWheelHardware(ILogger logger, IProfile profile)
        {
            Logger = logger;
            Profile = profile;

            moving = false;

            AllFilterNames = new string[8];
            AllFocusOffsets = new int[8];
            AllFilterColours = new Color[8];

            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        /// <summary>
        /// Time to move between filter positions (milliseconds)
        /// </summary>
        public Setting<int> FilterChangeTimeInterval { get; } = new Setting<int>("FilterChangeTimeInterval", "Time to move between filter positions (milliseconds)", 1000);

        public Setting<bool> ImplementsNames { get; } = new Setting<bool>("ImplementsNames", "True if the driver implements names", true);

        public Setting<bool> ImplementsOffsets { get; } = new Setting<bool>("ImplementsOffsets", "True if the driver implements offsets", true);

        public Setting<bool> PreemptMoves { get; } = new Setting<bool>("PreemptMoves", "True if the driver can interrupt moves", false);

        public Setting<int> Slots { get; } = new Setting<int>("Slots", "Number of filter wheel positions, 1-8", 6);

        public string[] AllFilterNames { get; set; }        // Array of filter name strings
        public int[] AllFocusOffsets { get; set; }         // Array of focus offsets
        public Color[] AllFilterColours { get; }       // Array of filter colors

        internal ILogger Logger
        {
            get;
            set;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            UpdateState();
        }

        #region Properties and Methods

        //
        // Initialize/finalize for server startup/shutdown
        //
        public void Initialize()
        {
            LoadSettings();
        }

        public short Position
        {
            get
            {
                lock (s_objSync)
                {
                    short ret;

                    if (moving)
                        ret = -1;                       // Spec. says we must return -1 if position not determined
                    else
                        ret = position;      // Otherwise return position

                    Logger.LogVerbose("FilterWheel - Get Position = " + ret.ToString());

                    return ret;
                }
            }
            set
            {
                lock (s_objSync)
                {
                    int Jumps;      // number of slot positions we have to move

                    Logger.LogInformation("FilterWheel - Set Position = " + value + " ...");

                    // position range check
                    if (value >= Slots.Value || value < 0)
                    {
                        Logger.LogError("FilterWheel -   (set position failed, out of range)");

                        throw new DriverException("Position: " + MSG_VAL_OUTOFRANGE, SCODE_VAL_OUTOFRANGE);
                    }

                    targetPosition = value;

                    // check if we are already there!
                    if (value == position)
                    {
                        Logger.LogInformation("FilterWheel -   (set position, no move required)");
                        return;
                    }

                    // check if we are already moving
                    if (moving)
                    {
                        if (PreemptMoves.Value)
                            AbortMove();   // Stop the motor
                        else
                        {
                            Logger.LogInformation("FilterWheel -   (set position failed, already moving)");

                            throw new DriverException("Position: " + MSG_MOVING, SCODE_MOVING);
                        }
                    }

                    // Find the shortest distance between two filter positions
                    Jumps = Math.Min(Math.Abs(position - targetPosition), Slots.Value - Math.Abs(position - targetPosition));
                    timeToMove = Jumps * FilterChangeTimeInterval.Value;

                    // trigger the "motor"
                    moving = true;

                    // log action
                    Logger.LogVerbose("FilterWheel -  (set position in progress)...");
                }
            }
        }

        public string[] FilterNames
        {
            get
            {
                Logger.LogVerbose("FilterWheel - Get FilterNames...");

                string[] temp = AllFilterNames;
                Array.Resize(ref temp, Slots.Value);
                for (int i = 0; i < Slots.Value; i++)
                    Logger.LogVerbose("FilterWheel -  Filter " + i.ToString() + " = " + temp[i].ToString());
                return temp;
            }
        }

        public int[] FocusOffsets
        {
            get
            {
                Logger.LogVerbose("FilterWheel - Get FocusOffsets..." + Environment.NewLine);
                int[] temp = AllFocusOffsets;
                Array.Resize(ref temp, Slots.Value);
                for (int i = 0; i < Slots.Value; i++)
                    Logger.LogVerbose("FilterWheel -  Offset " + i.ToString() + " = " + temp[i].ToString() + Environment.NewLine);
                return temp;
            }
        }

        public void AbortMove()
        {
            Logger.LogInformation("FilterWheel - Abort move");
            // Clear the elapsed time
            timeElapsed = 0;
            // Stop moving
            moving = false;
            // Set the postion intermediate between start and end
            position = (short)Math.Floor(Math.Abs(targetPosition - position) / 2.0);
            Logger.LogInformation("FilterWheel - Abort done");
        }

        public void UpdateState()
        {
            lock (s_objSync)
            {
                if (moving)
                {
                    // We are moving so increment the elapsed move time
                    timeElapsed += timerTickInterval;

                    // Have we reached the filter position yet?
                    if (timeElapsed >= timeToMove)
                    {
                        // Clear the elapsed time
                        timeElapsed = 0;
                        // Stop moving
                        moving = false;
                        // Set the new position
                        position = targetPosition;
                    }
                }
            }
        }

        #endregion Properties and Methods

        #region private utilities

        //
        // Get Settings from Registry
        //
        private void LoadSettings()
        {
            try
            {
                if (Profile.GetValue("RegVer", string.Empty) != m_sRegVer)
                {
                    SetDefaultSettings();
                }

                // Read the hardware & driver config
                Slots.Value = Convert.ToInt16(Profile.GetSettingReturningDefault(Slots));
                if (Slots.Value < 1 || Slots.Value > 8) Slots.Value = 6;
                position = 0;
                FilterChangeTimeInterval.Value = Convert.ToInt32(Profile.GetSettingReturningDefault(FilterChangeTimeInterval));
                ImplementsNames.Value = Convert.ToBoolean(Profile.GetSettingReturningDefault(ImplementsNames));
                ImplementsOffsets.Value = Convert.ToBoolean(Profile.GetSettingReturningDefault(ImplementsOffsets));
                PreemptMoves.Value = Convert.ToBoolean(Profile.GetSettingReturningDefault(PreemptMoves));
                for (int i = 0; i <= 7; i++)
                {
                    AllFilterNames[i] = Profile.GetValue($"FilterNames {i}");
                    AllFocusOffsets[i] = Convert.ToInt32(Profile.GetValue($"FocusOffsets {i}"));
                    AllFilterColours[i] = Color.FromName(Profile.GetValue($"Filter {i} Color", DefaultColors[i].Name));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"FilterWheel - {ex.Message}");
                SetDefaultSettings();
            }
        }

        private void SetDefaultSettings()
        {
            //
            // initialize variables that are not present
            //

            string[] names = new string[8] { "Red", "Green", "Blue", "Clear", "Ha", "OIII", "LPR", "Dark" };
            Random rand = new Random();

            Profile.WriteValue("RegVer", m_sRegVer);
            Profile.WriteValue("Position", "0");
            Profile.SetSettingDefault(Slots);
            Profile.SetSettingDefault(FilterChangeTimeInterval);
            Profile.SetSettingDefault(ImplementsNames);
            Profile.SetSettingDefault(ImplementsNames);
            Profile.SetSettingDefault(ImplementsOffsets);
            Profile.SetSettingDefault(PreemptMoves);
            for (int i = 0; i < 8; i++)
            {
                Profile.WriteValue($"FilterNames {i}", names[i]);
                Profile.WriteValue($"FocusOffsets {i}", rand.Next(10000).ToString());
                Profile.WriteValue($"Filter {i} Color", DefaultColors[i].Name);
            }
        }

        public void SaveSettings()
        {
            int i = 0;
            Profile.SetSetting(Slots);
            Profile.SetSetting(FilterChangeTimeInterval);
            for (i = 0; i <= 7; i++)
            {
                Profile.WriteValue($"FilterNames {i}", AllFilterNames[i]);
                Profile.WriteValue($"FocusOffsets {i}", AllFocusOffsets[i].ToString());
                Profile.WriteValue($"Filter {i} Color", AllFilterColours[i].Name);
            }
            Profile.SetSetting(ImplementsNames);
            Profile.SetSetting(ImplementsOffsets);
            Profile.SetSetting(PreemptMoves);

            LoadSettings();
        }

        #endregion private utilities
    }
}