namespace Instaq.API.Extern.Helpers
{
    using System.Reflection;

    public class VersionInfo
    {
        public static string Version
            => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                   ?.InformationalVersion ?? "";
    }
}
