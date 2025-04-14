namespace ASCOM.Simulators
{
    using System;
    using System.Timers;

    using ASCOM.Common.DeviceInterfaces;
    using ASCOM.Common.Interfaces;
    using OmniSim.BaseDriver;

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
        SlewHome = 4
    }

    public class DomeHardware
    {
        private IProfile Profile;

        public const double INVALID_COORDINATE = -100000.0;

        public double TIMER_INTERVAL { get; set; } = 0.25;        // seconds per tick
        public double PARK_HOME_TOL { get; set; } = 1.0;           // Tolerance (deg) for Park/Home position



        public double TargetAltitude { get; set; }               // Target Alt
        public double TargetAzimuth { get; set; }                // Target Az
        public double OCProgress { get; set; }              // Target Az

        public bool g_bAtHome { get; set; }                 // Home state
        public bool g_bAtPark { get; set; }                 // Park state
        public ShutterState ShutterState { get; set; }      // shutter status


        public Going Slewing { get; set; }                  // Move in progress

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
        public Setting<double> DomeAltitude { get; } = new Setting<double>("DomeAltitude", "The saved dome altitude", INVALID_COORDINATE);

        /// <summary>
        /// Gets the current dome azimuth.
        /// </summary>
        public Setting<double> DomeAzimuth { get; } = new Setting<double>("DomeAzimuth", "The saved dome azimuth", INVALID_COORDINATE);

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

        internal void LoadConfig()
        {
            OCProgress = 0;
            OCDelay.Value = Profile.GetSettingReturningDefault(OCDelay);
            ParkPosition.Value = Profile.GetSettingReturningDefault(ParkPosition);
            ParkPosition.Value = Profile.GetSettingReturningDefault(ParkPosition);
            AltitudeRate.Value = Profile.GetSettingReturningDefault(AltitudeRate);
            AzimuthRate.Value = Profile.GetSettingReturningDefault(AzimuthRate);
            MaximumAltitude.Value = Profile.GetSettingReturningDefault(MaximumAltitude);
            MinimumAltitude.Value = Profile.GetSettingReturningDefault(MinimumAltitude);
            StartWithShutterError.Value = Profile.GetSettingReturningDefault(StartWithShutterError);
            InterfaceVersionSetting.Value = Profile.GetSettingReturningDefault(InterfaceVersionSetting);

            SlewingTrueWhenOpenOrClose.Value = Profile.GetSettingReturningDefault(SlewingTrueWhenOpenOrClose);
            StandardAtHome.Value = Profile.GetSettingReturningDefault(StandardAtHome);
            StandardAtPark.Value = Profile.GetSettingReturningDefault(StandardAtPark);

            CanFindHome.Value = Profile.GetSettingReturningDefault(CanFindHome);
            CanPark.Value = Profile.GetSettingReturningDefault(CanPark);
            CanSetAltitude.Value = Profile.GetSettingReturningDefault(CanSetAltitude);
            CanSetAzimuth.Value = Profile.GetSettingReturningDefault(CanSetAzimuth);
            CanSetPark.Value = Profile.GetSettingReturningDefault(CanSetPark);
            CanSetShutter.Value = Profile.GetSettingReturningDefault(CanSetShutter);
            CanSyncAzimuth.Value = Profile.GetSettingReturningDefault(CanSyncAzimuth);

            DomeAzimuth.Value = Profile.GetSettingReturningDefault(DomeAzimuth);
            DomeAltitude.Value =  Profile.GetSettingReturningDefault(DomeAltitude);

            if (DomeAltitude.Value < MinimumAltitude.Value)
                DomeAltitude.Value = MinimumAltitude.Value;
            if (DomeAltitude.Value > MaximumAltitude.Value)
                DomeAltitude.Value = MaximumAltitude.Value;
            if (DomeAzimuth.Value < 0 | DomeAzimuth.Value >= 360)
                DomeAzimuth.Value = ParkPosition.Value;
            TargetAltitude = DomeAltitude.Value;
            TargetAzimuth = DomeAzimuth.Value;

            if (StartWithShutterError.Value)
                ShutterState = ShutterState.Error;
            else
            {
                string ret = Profile.GetSettingReturningDefault(ShutterStateSetting);       // ShutterClosed
                ShutterState = (ShutterState)Enum.Parse(typeof(ShutterState), ret.ToString());
            }

            Slewing = Going.SlewNowhere;
            g_bAtPark = AtPark;                   // its OK to wake up parked
            if (StandardAtHome.Value)
                g_bAtHome = false;                   // Standard: home is set by home() method, never wake up homed!
            else
                g_bAtHome = AtHome;// Non standard, position, OK to wake up homed
        }

        internal void SaveConfig()
        {
            Profile.SetSetting(OCDelay);
            Profile.SetSetting(ParkPosition);
            Profile.SetSetting(HomePosition);
            Profile.SetSetting(AltitudeRate);
            Profile.SetSetting(AzimuthRate);
            Profile.SetSetting(MaximumAltitude);
            Profile.SetSetting(MinimumAltitude);
            Profile.SetSetting(StartWithShutterError);
            Profile.SetSetting(InterfaceVersionSetting);
            Profile.SetSetting(SlewingTrueWhenOpenOrClose);
            Profile.SetSetting(StandardAtHome);
            Profile.SetSetting(StandardAtPark);

            Profile.SetSetting(DomeAzimuth);
            Profile.SetSetting(DomeAltitude);

            ShutterStateSetting.Value = System.Convert.ToString(ShutterState);

            Profile.SetSetting(ShutterStateSetting);

            Profile.SetSetting(CanFindHome);
            Profile.SetSetting(CanPark);
            Profile.SetSetting(CanSetAltitude);
            Profile.SetSetting(CanSetAzimuth);
            Profile.SetSetting(CanSetPark);
            Profile.SetSetting(CanSetShutter);
            Profile.SetSetting(CanSyncAzimuth);
        }


        private readonly Timer timer = new Timer(100)
        {
            AutoReset = true,
        };

        public DomeHardware(IProfile profile)
        {
            Profile = profile;
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

                return distanceFromPark < PARK_HOME_TOL;
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

                return distanceFromHome < PARK_HOME_TOL;
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
            g_bAtHome = false;
            g_bAtPark = false;
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
                g_bAtPark = false;
            }

            if (StandardAtHome.Value)
            {
                g_bAtHome = false;
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
            g_bAtHome = false;
            g_bAtPark = false;
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
            g_bAtHome = false;
            g_bAtPark = false;
            TargetAzimuth = ParkPosition.Value;
            Slewing = Going.SlewPark;
        }

        /// <summary>
        /// Spins the dome in a direction.
        /// </summary>
        /// <param name="clockwise">True for clockwise.</param>
        public void Run(bool clockwise)
        {
            g_bAtHome = false;
            g_bAtPark = false;
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
                g_bAtHome = false;
            }
            else
            {
                // Position (non-standard)
                g_bAtHome = AtHome;
            }

            if (StandardAtPark.Value)
            {
                // Fragile (standard)
                g_bAtPark = false;
            }
            else
            {
                // Position (non-standard)
                g_bAtPark = AtPark;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            double slew;
            double distance;

            // Azimuth slew simulation
            if (Slewing != Going.SlewNowhere)
            {
                slew = AzimuthRate.Value * TIMER_INTERVAL;
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
                            g_bAtHome = true;
                        }
                    }
                    else
                    {
                        // Position (non-standard)
                        g_bAtHome = AtHome;
                    }

                    if (StandardAtPark.Value)
                    {
                        if (Slewing == Going.SlewPark)
                        {
                            // Fragile (standard)
                            g_bAtPark = true;
                        }
                    }
                    else
                    {
                        // Position (non-standard)
                        g_bAtPark = AtPark;
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
                slew = AltitudeRate.Value * TIMER_INTERVAL;
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
                OCProgress = OCProgress - TIMER_INTERVAL;
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