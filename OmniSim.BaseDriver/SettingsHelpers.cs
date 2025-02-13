using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmniSim.BaseDriver
{
    public static class SettingsHelpers
    {
        public static System.Collections.Generic.IEnumerable<PropertyInfo> GetSettingsProperties(Type DriverType)
        {
            var types = DriverType.GetProperties();
            var props = types.Where(p => p.PropertyType.ToString().Contains(typeof(OmniSim.BaseDriver.Setting<>).FullName.ToString()));
            return props;
        }
    }
}
