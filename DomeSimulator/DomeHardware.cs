namespace ASCOM.Simulators
{
    using ASCOM.Common.DeviceInterfaces;
    using ASCOM.Common.Interfaces;
    using OmniSim.BaseDriver;
    using System;
    using System.Timers;

    /// <summary>
    /// The directions and states the dome can move in.
    /// </summary>
    public enum Going
    {
        /// <summary>
        /// Run counter clockwise until halt.
        /// </summary>
        SlewCCW = -1,

        /// <summary>
        /// Stopped, complete. not slewing.
        /// </summary>
        SlewNowhere = 0,

        /// <summary>
        /// Run clockwise until halt.
        /// </summary>
        SlewCW = 1,

        /// <summary>
        /// Slew to a specified Azimuth.
        /// </summary>
        SlewSomewhere = 2,

        /// <summary>
        /// Slew to park position.
        /// </summary>
        SlewPark = 3,

        /// <summary>
        /// Slew to the home position.
        /// </summary>
        SlewHome = 4,
    }

    /// <summary>
    /// The Dome Hardware simulation class
    /// </summary>
    public class DomeHardware
    {
        /// <summary>
        /// Constant Value for bad dome position
        /// </summary>
        public const double InvalidCoordinate = -100000.0;

        private readonly Timer timer = new Timer(100)
        {
            AutoReset = true,
        };

        /// <summary>
        /// Seconds per tick.
        /// </summary>
        private const double TimerInterval = 0.25;

        /// <summary>
        /// Tolerance (deg) for Park/Home position.
        /// </summary>
        private const double ParkHomeTolerance = 1.0;

        private readonly IProfile settingsProfile;

        /// <summary>
        /// Gets or Sets the Target Altitude.
        /// </summary>
        public double TargetAltitude { get; set; }

        /// <summary>
        /// Gets or Sets the Target Azimuth.
        /// </summary>
        public double TargetAzimuth { get; set; }

        /// <summary>
        /// Gets or Sets the Open Close Progress.
        /// </summary>
        public double OCProgress { get; set; }

        /// <summary>
        /// Gets or sets the Home State (ToDo maybe remove?).
        /// </summary>
        public bool CommandAtHome { get; set; }

        /// <summary>
        /// Gets or sets the Park State (ToDo maybe remove?).
        /// </summary>
        public bool CommandAtPark { get; set; }

        /// <summary>
        /// Gets or Sets the Shutter State.
        /// </summary>
        public ShutterState ShutterState { get; set; }

        /// <summary>
        /// Gets or Sets the Slewing State.
        /// </summary>
        public Going Slewing { get; set; }


        // State Variables

        /// <summary>
        /// Gets the altitude rate in degrees per second.
        /// </summary>
        public Setting<double> AltitudeRate { get; } = new Setting<double>("AltitudeRate", "The altitude rate in degrees per second", 30);

        /// <summary>
        /// Gets the azimuth rate in degrees per second.
        /// </summary>
        public Setting<double> AzimuthRate { get; } = new Setting<double>("AzimuthRate", "The azimuth rate in degrees per second", 15);

        /// <summary>
        /// Gets the current dome altitude.
        /// </summary>
        public Setting<double> DomeAltitude { get; } = new Setting<double>("DomeAltitude", "The saved dome altitude", InvalidCoordinate);

        /// <summary>
        /// Gets the current dome azimuth.
        /// </summary>
        public Setting<double> DomeAzimuth { get; } = new Setting<double>("DomeAzimuth", "The saved dome azimuth", InvalidCoordinate);

        /// <summary>
        /// Gets the maximum altitude in degrees.
        /// </summary>
        public Setting<double> MinimumAltitude { get; } = new Setting<double>("MinimumAltitude", "The minimum altitude in degrees", 0);

        /// <summary>
        /// Gets the maximum altitude in degrees.
        /// </summary>
        public Setting<double> MaximumAltitude { get; } = new Setting<double>("MaximumAltitude", "The maximum altitude in degrees", 90);

        /// <summary>
        /// Gets the Start with a shutter error.
        /// </summary>
        public Setting<bool> StartWithShutterError { get; } = new Setting<bool>("StartWithShutterError", "Start with a shutter error (legacy, non-standard)", false);

        /// <summary>
        /// Gets the Start with a shutter error.
        /// </summary>
        public Setting<bool> StandardAtHome { get; } = new Setting<bool>("StandardAtHome", "False (non-std) means AtHome true whenever az = home", true);

        /// <summary>
        /// Gets the Start with a shutter error.
        /// </summary>
        public Setting<bool> StandardAtPark { get; } = new Setting<bool>("StandardAtPark", "False (non-std) means AtPark true whenever az = home", true);

        /// <summary>
        /// Gets the Start with a shutter error.
        /// </summary>
        public Setting<bool> SlewingTrueWhenOpenOrClose { get; } = new Setting<bool>("SlewingTrueWhenOpenOrClose", "Slewing true when shutter opening/closing", false);

        /// <summary>
        /// Gets the dome park position.
        /// </summary>
        public Setting<double> ParkPosition { get; } = new Setting<double>("ParkPosition", "The dome park position", 180);

        /// <summary>
        /// Gets the dome Home position.
        /// </summary>
        public Setting<double> HomePosition { get; } = new Setting<double>("HomePosition", "The dome home position", 0);

        /// <summary>
        /// Gets the delay time for Open / Close.
        /// </summary>
        public Setting<double> OCDelay { get; } = new Setting<double>("OCDelay", "Delay time in seconds for Open / Close", 3);

        /// <summary>
        /// Gets the stored shutter state.
        /// </summary>
        public Setting<string> ShutterStateSetting { get; } = new Setting<string>("ShutterState", "The stored shutter state.", "1");

        // Dome Capabilities

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanFindHome { get; } = new Setting<bool>("CanFindHome", "True if the dome can find home", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanPark { get; } = new Setting<bool>("CanPark", "True if the dome can park", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanSetAltitude { get; } = new Setting<bool>("CanSetAltitude", "True if the dome can set it's altitude", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanSetAzimuth { get; } = new Setting<bool>("CanSetAzimuth", "True if the dome can set it's azimuth", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanSetPark { get; } = new Setting<bool>("CanSetPark", "True if the dome can set the park position", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanSetShutter { get; } = new Setting<bool>("CanSetShutter", "True if the dome can set the shutter", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<bool> CanSyncAzimuth { get; } = new Setting<bool>("CanSyncAzimuth", "True if the dome can sync azimuth", true);

        /// <summary>
        /// Gets the property flag.
        /// </summary>
        public Setting<short> ConnectDelay { get; } = new Setting<short>("ConnectDelay", "The delay to be used for Connect() in milliseconds, allowed values are 1-30000", 1500);

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-3", 3);

        /// <summary>
        /// Load settings from the Profile Storage.
        /// </summary>
        internal void LoadConfig()
        {
            OCProgress = 0;
            OCDelay.Value = settingsProfile.GetSettingReturningDefault(OCDelay);
            ParkPosition.Value = settingsProfile.GetSettingReturningDefault(ParkPosition);
            ParkPosition.Value = settingsProfile.GetSettingReturningDefault(ParkPosition);
            AltitudeRate.Value = settingsProfile.GetSettingReturningDefault(AltitudeRate);
            AzimuthRate.Value = settingsProfile.GetSettingReturningDefault(AzimuthRate);
            MaximumAltitude.Value = settingsProfile.GetSettingReturningDefault(MaximumAltitude);
            MinimumAltitude.Value = settingsProfile.GetSettingReturningDefault(MinimumAltitude);
            StartWithShutterError.Value = settingsProfile.GetSettingReturningDefault(StartWithShutterError);
            InterfaceVersionSetting.Value = settingsProfile.GetSettingReturningDefault(InterfaceVersionSetting);

            SlewingTrueWhenOpenOrClose.Value = settingsProfile.GetSettingReturningDefault(SlewingTrueWhenOpenOrClose);
            StandardAtHome.Value = settingsProfile.GetSettingReturningDefault(StandardAtHome);
            StandardAtPark.Value = settingsProfile.GetSettingReturningDefault(StandardAtPark);

            CanFindHome.Value = settingsProfile.GetSettingReturningDefault(CanFindHome);
            CanPark.Value = settingsProfile.GetSettingReturningDefault(CanPark);
            CanSetAltitude.Value = settingsProfile.GetSettingReturningDefault(CanSetAltitude);
            CanSetAzimuth.Value = settingsProfile.GetSettingReturningDefault(CanSetAzimuth);
            CanSetPark.Value = settingsProfile.GetSettingReturningDefault(CanSetPark);
            CanSetShutter.Value = settingsProfile.GetSettingReturningDefault(CanSetShutter);
            CanSyncAzimuth.Value = settingsProfile.GetSettingReturningDefault(CanSyncAzimuth);

            DomeAzimuth.Value = settingsProfile.GetSettingReturningDefault(DomeAzimuth);
            DomeAltitude.Value = settingsProfile.GetSettingReturningDefault(DomeAltitude);

            if (DomeAltitude.Value < MinimumAltitude.Value)
            {
                DomeAltitude.Value = MinimumAltitude.Value;
            }

            if (DomeAltitude.Value > MaximumAltitude.Value)
            {
                DomeAltitude.Value = MaximumAltitude.Value;
            }

            if (DomeAzimuth.Value < 0 | DomeAzimuth.Value >= 360)
            {
                DomeAzimuth.Value = ParkPosition.Value;
            }

            TargetAltitude = DomeAltitude.Value;
            TargetAzimuth = DomeAzimuth.Value;

            if (StartWithShutterError.Value)
            {
                ShutterState = ShutterState.Error;
            }
            else
            {
                // ShutterClosed
                string ret = settingsProfile.GetSettingReturningDefault(ShutterStateSetting);
                ShutterState = (ShutterState)Enum.Parse(typeof(ShutterState), ret.ToString());
            }

            Slewing = Going.SlewNowhere;
            // its OK to wake up parked
            CommandAtPark = AtPark;

            // Standard: home is set by home() method, never wake up homed!
            if (StandardAtHome.Value)
            {
                CommandAtHome = false;
            }
            else
            {
                CommandAtHome = AtHome;// Non standard, position, OK to wake up homed
            }
        }

        /// <summary>
        /// Save all settings to the Profile
        /// </summary>
        internal void SaveConfig()
        {
            settingsProfile.SetSetting(OCDelay);
            settingsProfile.SetSetting(ParkPosition);
            settingsProfile.SetSetting(HomePosition);
            settingsProfile.SetSetting(AltitudeRate);
            settingsProfile.SetSetting(AzimuthRate);
            settingsProfile.SetSetting(MaximumAltitude);
            settingsProfile.SetSetting(MinimumAltitude);
            settingsProfile.SetSetting(StartWithShutterError);
            settingsProfile.SetSetting(InterfaceVersionSetting);
            settingsProfile.SetSetting(SlewingTrueWhenOpenOrClose);
            settingsProfile.SetSetting(StandardAtHome);
            settingsProfile.SetSetting(StandardAtPark);

            settingsProfile.SetSetting(DomeAzimuth);
            settingsProfile.SetSetting(DomeAltitude);

            ShutterStateSetting.Value = System.Convert.ToString(ShutterState);

            settingsProfile.SetSetting(ShutterStateSetting);

            settingsProfile.SetSetting(CanFindHome);
            settingsProfile.SetSetting(CanPark);
            settingsProfile.SetSetting(CanSetAltitude);
            settingsProfile.SetSetting(CanSetAzimuth);
            settingsProfile.SetSetting(CanSetPark);
            settingsProfile.SetSetting(CanSetShutter);
            settingsProfile.SetSetting(CanSyncAzimuth);
        }

        /// <summary>
        /// Used only for reflection
        /// </summary>
        public DomeHardware()
        {

        }

        /// <summary>
        /// Create a Dome object with the provided profile.
        /// </summary>
        /// <param name="profile"></param>
        public DomeHardware(IProfile profile)
        {
            settingsProfile = profile;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        /// <summary>
        /// Gets a value indicating whether the dome is slewing.
        /// </summary>
        public bool IsSlewing
        {
            get
            {
                // Non-standard, Slewing true if shutter is opening/closing
                if (SlewingTrueWhenOpenOrClose.Value)
                {
                    return (Slewing != Going.SlewNowhere) | (DomeAltitude.Value != TargetAltitude) | (ShutterState == ShutterState.Closing) | (ShutterState == ShutterState.Opening);
                }
                else
                {
                    // slewing is true if either Alt or Az are in motion
                    return (Slewing != Going.SlewNowhere) | (DomeAltitude.Value != TargetAltitude);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the dome is parked.
        /// </summary>
        public bool AtPark
        {
            get
            {
                double distanceFromPark = AzScale(System.Math.Abs(DomeAzimuth.Value - ParkPosition.Value));
                if (distanceFromPark > 180)
                {
                    distanceFromPark = 360 - distanceFromPark;
                }

                return distanceFromPark < ParkHomeTolerance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the dome is at the home position.
        /// </summary>
        public bool AtHome
        {
            get
            {
                double distanceFromHome = AzScale(System.Math.Abs(DomeAzimuth.Value - HomePosition.Value));
                if (distanceFromHome > 180)
                {
                    distanceFromHome = 360 - distanceFromHome;
                }

                return distanceFromHome < ParkHomeTolerance;
            }
        }

        /// <summary>
        /// Scale the azimuth to the 0-360 range.
        /// </summary>
        /// <param name="azimuth">The value to range.</param>
        /// <returns>A valid value in range.</returns>
        public static double AzScale(double azimuth)
        {
            while (azimuth < 0.0)
            {
                azimuth = azimuth + 360.0;
            }

            while (azimuth >= 360.0)
            {
                azimuth = azimuth - 360.0;
            }

            return azimuth;
        }

        /// <summary>
        /// Closes the dome shutter.
        /// </summary>
        public void CloseShutter()
        {
            if (ShutterState == ShutterState.Closed)
            {
                return;
            }

            OCProgress = OCDelay.Value;
            ShutterState = ShutterState.Closing;
        }

        /// <summary>
        /// Sends the dome to the target home position.
        /// </summary>
        public void FindHome()
        {
            CommandAtHome = false;
            CommandAtPark = false;
            TargetAzimuth = HomePosition.Value;
            Slewing = Going.SlewHome;
        }

        /// <summary>
        /// Halts dome motion.
        /// </summary>
        public void Halt()
        {
            TargetAltitude = DomeAltitude.Value;
            Slewing = Going.SlewNowhere;

            // clear home / park (state is fragile in standard)
            if (StandardAtPark.Value)
            {
                CommandAtPark = false;
            }

            if (StandardAtHome.Value)
            {
                CommandAtHome = false;
            }

            // If the shutter is in motion, then cause it to jam
            if (OCProgress > 0)
            {
                OCProgress = 0;
                ShutterState = ShutterState.Error;
            }
        }

        /// <summary>
        /// Moves the dome to a target azimuth.
        /// </summary>
        /// <param name="azimuth">The target azimuth.</param>
        public void Move(double azimuth)
        {
            CommandAtHome = false;
            CommandAtPark = false;
            TargetAzimuth = azimuth;
            Slewing = Going.SlewSomewhere;
        }

        /// <summary>
        /// Move the shutter to a target altitude.
        /// </summary>
        /// <param name="altitude">The target altitude.</param>
        public void MoveShutter(double altitude)
        {
            // If the shutter is opening or closing, then cause it to jam
            if (OCProgress > 0)
            {
                OCProgress = 0;
                ShutterState = ShutterState.Error;
            }
            else
            {
                TargetAltitude = altitude;
            }
        }

        /// <summary>
        /// Opens the shutter.
        /// </summary>
        public void OpenShutter()
        {
            // Ensure that the Alt stays in bounds
            if (MinimumAltitude.Value > DomeAltitude.Value)
            {
                TargetAltitude = MinimumAltitude.Value;
                DomeAltitude.Value = MinimumAltitude.Value;
            }

            if (MaximumAltitude.Value < DomeAltitude.Value)
            {
                TargetAltitude = MaximumAltitude.Value;
                DomeAltitude.Value = MaximumAltitude.Value;
            }

            if (ShutterState == ShutterState.Open)
            {
                return;
            }

            if (ShutterState == ShutterState.Error)
            {
                return;
            }

            OCProgress = OCDelay.Value;
            ShutterState = ShutterState.Opening;
        }

        /// <summary>
        /// Parks the dome.
        /// </summary>
        public void Park()
        {
            CommandAtHome = false;
            CommandAtPark = false;
            TargetAzimuth = ParkPosition.Value;
            Slewing = Going.SlewPark;
        }

        /// <summary>
        /// Spins the dome in a direction.
        /// </summary>
        /// <param name="clockwise">True for clockwise.</param>
        public void Run(bool clockwise)
        {
            CommandAtHome = false;
            CommandAtPark = false;
            Slewing = clockwise ? Going.SlewCW : Going.SlewCCW;
        }

        /// <summary>
        /// Syncs the Dome to the specified Azimuth.
        /// </summary>
        /// <param name="azimuth">The requested Azimuth.</param>
        public void Sync(double azimuth)
        {
            Slewing = Going.SlewNowhere;
            TargetAzimuth = azimuth;
            DomeAzimuth.Value = TargetAzimuth;

            // Handle standard (fragile) and non-standard park/home changes
            if (StandardAtHome.Value)
            {
                // Fragile (standard)
                CommandAtHome = false;
            }
            else
            {
                // Position (non-standard)
                CommandAtHome = AtHome;
            }

            if (StandardAtPark.Value)
            {
                // Fragile (standard)
                CommandAtPark = false;
            }
            else
            {
                // Position (non-standard)
                CommandAtPark = AtPark;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            double slew;
            double distance;

            // Azimuth slew simulation
            if (Slewing != Going.SlewNowhere)
            {
                slew = AzimuthRate.Value * TimerInterval;
                if (Slewing > Going.SlewCW)
                {
                    distance = TargetAzimuth - DomeAzimuth.Value;
                    if (distance < 0)
                    {
                        slew = -slew;
                    }

                    if (distance > 180)
                    {
                        slew = -slew;
                    }

                    if (distance < -180)
                    {
                        slew = -slew;
                    }
                }
                else
                {
                    distance = slew * 2;
                    slew = slew * (int)Slewing;
                }

                // Are we there yet ?
                if (System.Math.Abs(distance) < System.Math.Abs(slew))
                {
                    DomeAzimuth.Value = TargetAzimuth;

                    // Handle standard (fragile) and non-standard park/home changes
                    if (StandardAtHome.Value)
                    {
                        if (Slewing == Going.SlewHome)
                        {
                            // Fragile (standard)
                            CommandAtHome = true;
                        }
                    }
                    else
                    {
                        // Position (non-standard)
                        CommandAtHome = AtHome;
                    }

                    if (StandardAtPark.Value)
                    {
                        if (Slewing == Going.SlewPark)
                        {
                            // Fragile (standard)
                            CommandAtPark = true;
                        }
                    }
                    else
                    {
                        // Position (non-standard)
                        CommandAtPark = AtPark;
                    }

                    Slewing = Going.SlewNowhere;
                }
                else
                {
                    DomeAzimuth.Value = AzScale(DomeAzimuth.Value + slew);
                }
            }

            // shutter altitude control simulation
            if (DomeAltitude.Value != TargetAltitude)
            {
                slew = AltitudeRate.Value * TimerInterval;
                distance = TargetAltitude - DomeAltitude.Value;
                if (distance < 0)
                {
                    slew = -slew;
                }

                // Are we there yet ?
                if (System.Math.Abs(distance) < System.Math.Abs(slew))
                {
                    DomeAltitude.Value = TargetAltitude;
                }
                else
                {
                    DomeAltitude.Value = DomeAltitude.Value + slew;
                }
            }

            // shutter open/close simulation
            if (OCProgress > 0)
            {
                OCProgress = OCProgress - TimerInterval;
                if (OCProgress <= 0)
                {
                    if (ShutterState == ShutterState.Opening)
                    {
                        ShutterState = ShutterState.Open;
                    }
                    else
                    {
                        ShutterState = ShutterState.Closed;
                    }
                }
            }
        }
    }
}