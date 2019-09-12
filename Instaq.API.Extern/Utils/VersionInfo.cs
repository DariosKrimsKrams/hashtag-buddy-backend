using System.Reflection;

namespace Instaq.API.Extern.Utils
{
    public class VersionInfo
    {
        public static string Version
        {
            get
            {
                return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            }
        }
    }
}
