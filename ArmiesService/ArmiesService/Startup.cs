using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArmiesService.Initialization;

namespace ArmiesService
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(configuration)
                    .AddHttpContextAccessor()
                    .AddQueries()
                    .AddCommands()
                    .AddCache(configuration)
                    .AddQueue(configuration)
                    .AddConsumers()
                    .AddDatabase(configuration)
                    .AddDomain()
                    .AddLog()
                    .AddCommon()
                    .AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication()
               .UseMvc();
        }
    }
}
