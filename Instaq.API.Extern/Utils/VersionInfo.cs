namespace Instaq.API.Extern.Utils
{
    using System.Reflection;

    public class VersionInfo
    {
        public static string Version
            => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                   ?.InformationalVersion ?? "";
    }
}
