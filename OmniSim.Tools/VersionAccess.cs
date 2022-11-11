using System;
using System.Linq;
using System.Reflection;

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