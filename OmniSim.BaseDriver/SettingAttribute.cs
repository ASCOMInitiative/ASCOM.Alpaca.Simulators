using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmniSim.BaseDriver
{
    public class SettingAttribute : Attribute
    {
        public string Description
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public SettingAttribute(string key, string description)
        {
            Key = key;
            Description = description;
        }
    }

    public static class Extensions
    {
        public static string GetSettingKey<T>(this object Property)
        {
            return typeof(T).GetProperty(Property.GetType().Name)
                             .GetCustomAttribute<SettingAttribute>().Key;
        }
    }
}