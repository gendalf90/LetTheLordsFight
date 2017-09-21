using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UsersService.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UsersService
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

            var jwtSettings = Configuration.GetSection("Jwt");

            services.Configure<JwtOptions>(settings =>
            {
                //settings.Issuer = jwtSettings["Issuer"];

                //settings.Audience = jwtSettings["Audience"];

                //settings.ValidTime = TimeSpan.FromSeconds(jwtSettings.GetValue<double>("ValidTime"));

                settings.Sign = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256);
            });
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseBasicAuthentication();

            //var jwtSettings = Configuration.GetSection("Jwt");

            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //ValidIssuer = jwtSettings["Issuer"],
            //        ValidateIssuer = false,
            //        //ValidAudience = jwtSettings["Audience"],
            //        ValidateAudience = false,
            //        IssuerSigningKey = GetSecurityKey(),
            //        ValidateIssuerSigningKey = true,
            //        //RequireExpirationTime = true,
            //        //ValidateLifetime = true
            //    }
            //});

            app.UseMvc();
        }

        private SymmetricSecurityKey GetSecurityKey()
        {
            var sign = Configuration["TOKEN_SIGNING_KEY"];
            var bytes = Encoding.ASCII.GetBytes(sign);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
