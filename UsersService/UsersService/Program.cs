using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Builder;
using System;

namespace UsersService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom
                                       .Configuration(hostingContext.Configuration)
                                       .Enrich
                                       .WithMachineName()
                                       .Enrich
                                       .WithProcessId()
                                       .Enrich
                                       .WithProperty("ApplicationName", "UsersService")
                                       .WriteTo
                                       .Async(configuration => configuration.File(@"Logs\Log.txt",
                                                                                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {MachineName} {ProcessId} {ApplicationName} {Message}{NewLine}{Exception}",
                                                                                  fileSizeLimitBytes: 1L * 100 * 1024 * 1024,
                                                                                  rollOnFileSizeLimit: true,
                                                                                  retainedFileCountLimit: 10,
                                                                                  flushToDiskInterval: TimeSpan.FromSeconds(1)),
                                              blockWhenFull: true);

                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
