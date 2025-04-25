using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmniSim.BaseDriver
{
    public class Setting<T>(string key, string description)
    {
        public T Value { get; set; }

        public T DefaultValue { get; set; } = default;

        public string Description
        {
            get; set;
        } = description;

        public string Key
        {
            get; set;
        } = key;

        public Setting(string key, string description, T default_value) : this(key, description)
        {
            DefaultValue = default_value;
            Value = default_value;
        }

        public override string ToString()
        {
            return SettingsExtensions.CultureSafeString(this.Value);
        }

    }

    public static class SettingsExtensions
    {
        public static void SetSetting<T>(this ASCOM.Common.Interfaces.IProfile Profile, Setting<T> Setting)  
        {
            Profile.WriteValue(Setting.Key, CultureSafeString(Setting.Value));
        }

        public static void SetSettingDefault<T>(this ASCOM.Common.Interfaces.IProfile Profile, Setting<T> Setting)
        {
            Profile.WriteValue(Setting.Key, CultureSafeString(Setting.DefaultValue));
        }

        internal static string CultureSafeString<T>(T value)
        {
            if (value is float)
            {
                return Convert.ToSingle(value).ToString(CultureInfo.InvariantCulture);
            }
            else if (value is double)
            {
                return Convert.ToDouble(value).ToString(CultureInfo.InvariantCulture);
            }
            else if (value is bool)
            {
                return Convert.ToBoolean(value).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetSettingReturningDefault(this ASCOM.Common.Interfaces.IProfile Profile, Setting<string> Setting)
        {
            return Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString());
        }

        public static float GetSettingReturningDefault(this ASCOM.Common.Interfaces.IProfile Profile, Setting<float> Setting)
        {
            return Convert.ToSingle(Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString()), CultureInfo.InvariantCulture);
        }

        public static double GetSettingReturningDefault(this ASCOM.Common.Interfaces.IProfile Profile, Setting<double> Setting)
        {
            return Convert.ToDouble(Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString()), CultureInfo.InvariantCulture);
        }

        public static bool GetSettingReturningDefault(this ASCOM.Common.Interfaces.IProfile Profile, Setting<bool> Setting)
        {
            return Convert.ToBoolean(Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString()), CultureInfo.InvariantCulture);
        }

        public static int GetSettingReturningDefault(this ASCOM.Common.Interfaces.IProfile Profile, Setting<int> Setting)
        {
            return Convert.ToInt32(Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString()), CultureInfo.InvariantCulture);
        }

        public static short GetSettingReturningDefault(this ASCOM.Common.Interfaces.IProfile Profile, Setting<short> Setting)
        {
            return Convert.ToInt16(Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString()), CultureInfo.InvariantCulture);
        }
    }
}
