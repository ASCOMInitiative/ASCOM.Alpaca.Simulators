using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmniSim.BaseDriver
{
    public class Setting<T>
    {
        public T Value { get; set; }

        public T DefaultValue { get; set; } = default;

        public string Description
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public Setting(string key, string description)
        {
            Key = key;
            Description = description;
        }

        public Setting(string key, string description, T default_value) : this(key, description)
        {
            DefaultValue = default_value;
        }

    }

    public static class SettingsExtensions
    {
        public static void SetSetting<T>(this ASCOM.Common.Interfaces.IProfile Profile, Setting<T> Setting)
        {
            Profile.WriteValue(Setting.Key, Setting.Value.ToString());
        }

        public static void SetSettingDefault<T>(this ASCOM.Common.Interfaces.IProfile Profile, Setting<T> Setting)
        {
            Profile.WriteValue(Setting.Key, Setting.DefaultValue.ToString());
        }

        public static string GetSettingReturningDefault<T>(this ASCOM.Common.Interfaces.IProfile Profile, Setting<T> Setting)
        {
            return Profile.GetValue(Setting.Key, Setting.DefaultValue.ToString());
        }
    }
}
