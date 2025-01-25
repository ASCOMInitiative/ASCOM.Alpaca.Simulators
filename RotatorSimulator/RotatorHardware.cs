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

        private float speed;

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
        /// Gets the stored interface version to use.
        /// </summary>
        public Setting<short> InterfaceVersionSetting { get; } = new Setting<short>("InterfaceVersion", "The ASCOM Interface Version, allowed values are 1-4", 4);

        public Setting<float> Position { get; } = new Setting<float>("Position", "The ASCOM Interface Version, allowed values are 1-4", 0);
        public Setting<double> RotationRate { get; } = new Setting<double>("RotationRate", "The ASCOM Interface Version, allowed values are 1-4", 3.0);
        public Setting<bool> CanReverse { get; } = new Setting<bool>("CanReverse", "The ASCOM Interface Version, allowed values are 1-4", true);
        public Setting<bool> Reverse { get; } = new Setting<bool>("Reverse", "The ASCOM Interface Version, allowed values are 1-4", false);
        public Setting<float> SyncOffset { get; } = new Setting<float>("SyncOffset", "The ASCOM Interface Version, allowed values are 1-4", 0);

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.UpdateState();
        }

        /// <summary>
        /// Load default values.
        /// </summary>
        /// <param name="profile"></param>
        public void Initialize(IProfile profile)
        {
            this.profile = profile;
            LoadProfile();
        }

        internal void LoadProfile()
        {
            this.Position.Value = Convert.ToSingle(this.profile.GetSettingReturningDefault(this.Position), CultureInfo.InvariantCulture);
            this.targetPosition = this.Position.Value;
            this.RotationRate.Value = Convert.ToSingle(this.profile.GetSettingReturningDefault(this.RotationRate), CultureInfo.InvariantCulture);
            this.CanReverse.Value = Convert.ToBoolean(this.profile.GetSettingReturningDefault(this.CanReverse));
            this.Reverse.Value = Convert.ToBoolean(this.profile.GetSettingReturningDefault(this.Reverse));
            this.SyncOffset.Value = Convert.ToSingle(this.profile.GetSettingReturningDefault(this.SyncOffset), CultureInfo.InvariantCulture);
        }

        public void ResetProfile()
        {
            profile.Clear();
        }

        public void SaveProfile(double rate, bool canreverse, bool reverse, float offset)  // "Finalize" exists in parent
        {
            profile.WriteValue("RotationRate.Value", rate.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue("CanReverse", canreverse.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue("Reverse", reverse.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue("SyncOffset", offset.ToString(CultureInfo.InvariantCulture));

            RotationRate.Value = (float)rate;
            CanReverse.Value = canreverse;
            Reverse.Value = reverse;
            SyncOffset.Value = offset;
        }

        public bool Connected
        {
            get;
            set;
        } = false;

        public float TargetPosition
        {
            get { CheckConnected(); return targetPosition; }
            set
            {
                CheckConnected();
                lock (objSync)
                {
                    targetPosition = value;
                    moving = true;                                   // Avoid timing window!(typ.)
                }
            }
        }

        public bool Moving
        {
            get { CheckConnected(); lock (objSync) { return moving; } }
        }

        public float StepSize
        {
            get { return speed * UpdateInterval; }
        }

        //
        // Methods for clients
        //
        public void Move(float relativePosition)
        {
            CheckConnected();
            lock (objSync)
            {
                // add check for relative position limits rather than using the check on the target.
                if (relativePosition <= -360.0 || relativePosition >= 360.0)
                {
                    throw new ASCOM.InvalidValueException("Relative Angle out of range", relativePosition.ToString(), "-360 < angle < 360");
                }
                var target = targetPosition + relativePosition;
                // force to the range 0 to 360
                if (target >= 360.0) target -= 360.0F;
                if (target < 0.0) target += 360.0F;
                targetPosition = target;
                moving = true;
            }
        }

        public void MoveAbsolute(float position)
        {
            CheckConnected();
            CheckAngle(position);
            lock (objSync)
            {
                targetPosition = position;
                moving = true;
            }
        }

        public void Halt()
        {
            lock (objSync)
            {
                targetPosition = Position.Value;
                moving = false;
            }
        }

        public void UpdateState()
        {
            lock (objSync)
            {
                float dPA = RangeAngle(targetPosition - Position.Value, -180, 180);
                if (Math.Abs(dPA) == 0)
                {
                    moving = false;
                    return;
                }
                //
                // Must move
                //
                float fDelta = speed * UpdateInterval;
                if (Position.Value == 180)                                             // Inhibit sneaking past 180
                {
                    if (direction && Math.Sign(dPA) > 0)
                        dPA = -1;
                    else if (!direction && Math.Sign(dPA) < 0)
                        dPA = 1;
                }
                if (dPA > 0 && Position.Value < 180 && RangeAngle((Position.Value + dPA), 0, 360) > 180)
                    Position.Value -= fDelta;
                else if (dPA < 0 && Position.Value > 180 && RangeAngle((Position.Value + dPA), 0, 360) < 180)
                    Position.Value += fDelta;
                else if (Math.Abs(dPA) >= fDelta)
                    Position.Value += (fDelta * Math.Sign(dPA));
                else
                    Position.Value += dPA;
                Position.Value = RangeAngle(Position.Value, 0, 360);
                direction = Math.Sign(dPA) > 0;                                  // Remember last direction for 180 check
                moving = true;
            }
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
    }
}