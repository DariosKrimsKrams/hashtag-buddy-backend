namespace Instaq.API.Extern
{
    using System.IO;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env          = hostingContext.HostingEnvironment;
                    var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");
                    var path         = Path.Combine(sharedFolder, "SharedSettings.json");
                    var pathEnv      = Path.Combine(sharedFolder, $"SharedSettings.{env.EnvironmentName}.json");
                    config
                        .AddJsonFile(path, true)
                        .AddJsonFile(pathEnv, true)
                        .AddJsonFile("appsettings.json", true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                    config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
    }
}
