using FluentMigrator.Runner;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AgerDeviceAPI
{
    public class Program
    {
        private static IConfigurationRoot GetConfigurationSettings()
        {
            try
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
            catch
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("AgerDeviceAPI/appsettings.json")
                    .Build();
            }
        }

        public static async Task Main(string[] args)
        {
            IConfigurationRoot config = GetConfigurationSettings();

            IHost apiHost = CreateHostBuilder(config, args).Build();

            using (IServiceScope scope = apiHost.Services.CreateScope())
            {
                IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            
            await apiHost.RunAsync();
        }

        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }

        public static IHostBuilder CreateHostBuilder(IConfigurationRoot config, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(config["server:urls"]);
                });
    }
}
