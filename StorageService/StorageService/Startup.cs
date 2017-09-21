using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using StorageService.Extensions;

namespace StorageService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();

            var mongoConnectionString = Configuration["MONGO_CONNECTION_STRING"];

            var servicesConfiguration = new ServicesConfiguration
            {
                ApiUrl = Configuration["API_URL"],
                Login = Configuration["STORAGE_SERVICE_LOGIN"],
                Password = Configuration["STORAGE_SERVICE_PASSWORD"]
            };

            var eventsConfiguration = new EventsConfiguration
            {
                IsOldSeconds = Configuration.GetValue<int>("Events:IsOldSeconds"),
                MakeSnapshotLimit = Configuration.GetValue<int>("Events:MakeSnapshotLimit")
            };

            services.AddMongo(mongoConnectionString)
                    .AddEvents(eventsConfiguration)
                    .AddServices(servicesConfiguration)
                    .AddStorages()
                    .AddDomain()
                    .AddCommands()
                    .AddQueries();

            //services.AddSingleton<ISystemClock, SystemClock>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var sign = Configuration["TOKEN_SIGNING_KEY"];

            app.UseTokens(sign);
            app.UseMvc();
        }
    }
}
