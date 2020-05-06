namespace Instaq.API.Extern
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.PlatformAbstractions;

    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env          = hostingContext.HostingEnvironment;
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var path = "SharedSettings.json";
                    var pathEnv = $"SharedSettings.{env.EnvironmentName}.json";
                    config
                        .SetBasePath(basePath)
                        .AddJsonFile(path, true)
                        .AddJsonFile(pathEnv, true)
                        .AddJsonFile("appsettings.json", true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                    config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
                .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
    }
}
