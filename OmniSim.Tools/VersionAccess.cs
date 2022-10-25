using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OmniSim.Tools
{
    public class VersionAccess
    {
        public static string GetVersionFromType(Type T)
        {
            return T.GetTypeInfo().Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().First().InformationalVersion;
        }
    }
}
