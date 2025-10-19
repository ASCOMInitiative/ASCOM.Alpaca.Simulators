namespace ASCOM.Simulators
{
    using System;
    using System.Globalization;
    using System.Timers;

    using ASCOM.Common.Interfaces;
    using OmniSim.BaseDriver;

    /// <summary>
    /// A Reference implementation of the ASCOM Rotator Specification used for Simulation.
    /// </summary>
    public class RotatorHardware
    {
        private const int UpdateInterval = 250;

        private bool moving;
        private bool direction;
        private float targetPosition;
        private object objSync = new object();

        private Timer timer = new Timer(100)
        {
            AutoReset = true,
        };

        private IProfile profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="RotatorHardware"/> class.
        /// </summary>
        public RotatorHardware()
        {
            this.Position.Value = 0.0F;
            this.Connected = false;
            this.moving = false;
            this.targetPosition = 0.0F;
            this.timer.Elapsed += this.OnTimedEvent;
            this.timer.Start();
        }

        /// <summary>
        /// Gets the delay for the connect timer
        /// </summary>
        public Setting<short> ConnectDelay { get; } = new Setting<short>("ConnectDelay", "The delay to be used for Connect() in milliseconds, allowed values are 1-30000", 1500);

        /// <summary>
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-4", 4);

        /// <summary>
        /// Gets the stored starting, between 0 and 360.
        /// </summary>
        public Setting<float> Position { get; } = new Setting<float>("Position", "The starting position 0 to 359.999", 0);

        /// <summary>
        /// Gets the rotation rate in degrees per second.
        /// </summary>
        public Setting<double> RotationRate { get; } = new Setting<double>("RotationRate", "The rotation rate in degrees per second", 3.0);

        /// <summary>
        /// Gets a value indicating if the rotator can reverse.
        /// </summary>
        public Setting<bool> CanReverse { get; } = new Setting<bool>("CanReverse", "True if the rotator can reverse", true);

        /// <summary>
        /// Gets a value indicating if the rotator is reversed.
        /// </summary>
        public Setting<bool> Reverse { get; } = new Setting<bool>("Reverse", "True if the rotator is reversed", false);

        /// <summary>
        /// Gets the current Sync Offset, 0 to 360.
        /// </summary>
        public Setting<float> SyncOffset { get; } = new Setting<float>("SyncOffset", "The current Sync Offset, 0 to 359.999", 0);

        /// <summary>
        /// Gets or sets a value indicating whether the simulator is connected.
        /// </summary>
        public bool Connected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value for the target position.
        /// </summary>
        public float TargetPosition
        {
            get
            {
                this.CheckConnected();
                return this.targetPosition;
            }

            set
            {
                this.CheckConnected();
                lock (this.objSync)
                {
                    this.targetPosition = value;
                    this.moving = true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the rotator is moving.
        /// </summary>
        public bool Moving
        {
            get
            {
                this.CheckConnected();
                lock (this.objSync)
                {
                    return this.moving;
                }
            }
        }

        /// <summary>
        /// Gets the current step size.
        /// </summary>
        public float StepSize
        {
            get
            {
                return Convert.ToSingle(this.RotationRate.Value / 1000 * UpdateInterval);
            }
        }

        /// <summary>
        /// Load default values.
        /// </summary>
        /// <param name="profile">Profile to use to store settings.</param>
        public void Initialize(IProfile profile)
        {
            this.profile = profile;
            this.LoadProfile();
        }

        /// <summary>
        /// Resets the profile and loads defaults.
        /// </summary>
        public void ResetProfile()
        {
            this.profile.Clear();
            this.LoadProfile();
        }

        /// <summary>
        /// Saves the settings to the profile and updates the value.
        /// </summary>
        /// <param name="rate">Rotation Rate.</param>
        /// <param name="canreverse">If the rotator can reverse.</param>
        /// <param name="reverse">If the rotator is reversed.</param>
        /// <param name="offset">The current sync offset.</param>
        public void SaveProfile(double rate, bool canreverse, bool reverse, float offset, short interfaceversion)
        {
            this.RotationRate.Value = (float)rate;
            this.CanReverse.Value = canreverse;
            this.Reverse.Value = reverse;
            this.SyncOffset.Value = offset;
            this.InterfaceVersionSetting.Value = interfaceversion;

            this.profile.SetSetting(this.RotationRate);
            this.profile.SetSetting(this.CanReverse);
            this.profile.SetSetting(this.Reverse);
            this.profile.SetSetting(this.SyncOffset);
            this.profile.SetSetting(this.InterfaceVersionSetting);
        }

        /// <summary>
        /// Moves the rotator by an offset.
        /// </summary>
        /// <param name="relativePosition">The target position.</param>
        /// <exception cref="ASCOM.InvalidValueException">If the value is out of range.</exception>
        public void Move(float relativePosition)
        {
            this.CheckConnected();
            lock (this.objSync)
            {
                // add check for relative position limits rather than using the check on the target.
                if (relativePosition <= -360.0 || relativePosition >= 360.0)
                {
                    throw new ASCOM.InvalidValueException($"Relative Angle out of range {relativePosition.ToString()} -360 < angle < 360");
                }

                var target = this.targetPosition + relativePosition;

                // force to the range 0 to 360
                if (target >= 360.0)
                {
                    target -= 360.0F;
                }

                if (target < 0.0)
                {
                    target += 360.0F;
                }

                this.targetPosition = target;
                this.moving = true;
            }
        }

        /// <summary>
        /// Moves the rotator to an absolute angle.
        /// </summary>
        /// <param name="position">Absolute target.</param>
        public void MoveAbsolute(float position)
        {
            this.CheckConnected();
            this.CheckAngle(position);
            lock (this.objSync)
            {
                this.targetPosition = position;
                this.moving = true;
            }
        }

        /// <summary>
        /// Halts an active move.
        /// </summary>
        public void Halt()
        {
            lock (this.objSync)
            {
                this.targetPosition = this.Position.Value;
                this.moving = false;
            }
        }

        /// <summary>
        /// Update status.
        /// </summary>
        public void UpdateState()
        {
            lock (this.objSync)
            {
                float dPA = this.RangeAngle(this.targetPosition - this.Position.Value, -180, 180);
                if (Math.Abs(dPA) == 0)
                {
                    this.moving = false;
                    return;
                }

                // Must move
                float fDelta = Convert.ToSingle(this.RotationRate.Value / 1000 * UpdateInterval);

                // Inhibit sneaking past 180
                if (this.Position.Value == 180)
                {
                    if (this.direction && Math.Sign(dPA) > 0)
                    {
                        dPA = -1;
                    }
                    else if (!this.direction && Math.Sign(dPA) < 0)
                    {
                        dPA = 1;
                    }
                }

                if (dPA > 0 && this.Position.Value < 180 && this.RangeAngle(this.Position.Value + dPA, 0, 360) > 180)
                {
                    this.Position.Value -= fDelta;
                }
                else if (dPA < 0 && this.Position.Value > 180 && this.RangeAngle(this.Position.Value + dPA, 0, 360) < 180)
                {
                    this.Position.Value += fDelta;
                }
                else if (Math.Abs(dPA) >= fDelta)
                {
                    this.Position.Value += fDelta * Math.Sign(dPA);
                }
                else
                {
                    this.Position.Value += dPA;
                }

                this.Position.Value = this.RangeAngle(this.Position.Value, 0, 360);

                // Remember last direction for 180 check
                this.direction = Math.Sign(dPA) > 0;
                this.moving = true;
            }
        }

        /// <summary>
        /// Loads stored settings or defaults from the profile.
        /// </summary>
        internal void LoadProfile()
        {
            this.Position.Value = this.profile.GetSettingReturningDefault(this.Position);
            this.targetPosition = this.Position.Value;
            this.RotationRate.Value = this.profile.GetSettingReturningDefault(this.RotationRate);
            this.CanReverse.Value = this.profile.GetSettingReturningDefault(this.CanReverse);
            this.Reverse.Value = this.profile.GetSettingReturningDefault(this.Reverse);
            this.SyncOffset.Value = this.profile.GetSettingReturningDefault(this.SyncOffset);
            this.InterfaceVersionSetting.Value = this.profile.GetSettingReturningDefault(this.InterfaceVersionSetting);
        }

        /// <summary>
        /// Checks if the rotator hardware is set to connected.
        /// </summary>
        /// <exception cref="NotConnectedException">Thrown if not connected.</exception>
        private void CheckConnected()
        {
            if (!this.Connected)
            {
                throw new NotConnectedException("The rotator is not connected");
            }
        }

        private void CheckAngle(float angle)
        {
            if (angle < 0.0F || angle >= 360.0F)
            {
                throw new ASCOM.InvalidValueException("Angle out of range", angle.ToString(), "0 <= angle < 360");
            }
        }

        private float RangeAngle(float angle, float min, float max)
        {
            while (angle >= max)
            {
                angle -= 360.0F;
            }

            while (angle < min)
            {
                angle += 360.0F;
            }

            return angle;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.UpdateState();
        }
    }
}