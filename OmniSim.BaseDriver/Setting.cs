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

        private static string CultureSafeString<T>(T value)
        {
            if (value is float)
            {
                return Convert.ToSingle(value).ToString(CultureInfo.InvariantCulture);
            }
            else if (value is double)
            {
                return Convert.ToSingle(value).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetSettingReturningDefault<T>(this ASCOM.Common.Interfaces.IProfile Profile, Setting<T> Setting)
        {
            return Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString());
        }
    }
}
