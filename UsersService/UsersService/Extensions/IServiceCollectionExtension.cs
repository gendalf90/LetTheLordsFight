using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IQueryFactory = UsersService.Queries.IFactory;
using QueryFactory = UsersService.Queries.Factory;
using ICommandFactory = UsersService.Commands.IFactory;
using CommandFactory = UsersService.Commands.Factory;
using UsersService.Options;
using ZNetCS.AspNetCore.Authentication.Basic;
using UsersService.BasicAuthentication;
using UsersDomain.Repositories;
using UsersService.Domain;
using UsersDomain.Services.Registration;
using UsersService.Commands.CreateRegistrationRequest;
using UsersService.Logs;
using UsersService.Queries.GetCurrentToken;

namespace UsersService.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                           .AddTransient<IGetTokenSigningKeyStrategy, GetTokenSigningKeyStrategy>()
                           .AddTransient<IGetCurrentUserStrategy, GetCurrentUserStrategy>()
                           .AddTransient<IQueryFactory, QueryFactory>();
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.AddTransient<ICommandFactory, CommandFactory>()
                           .AddTransient<IConfirmationUrl, ConfirmationUrl>();
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddTransient<IUsers, UsersRepository>()
                           .AddTransient<IEmail, EmailService>();
        }

        public static IServiceCollection AddLog(this IServiceCollection services)
        {
            return services.AddTransient<ILog, Log>();
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(options =>
            {
                options.SigningKey = configuration["TOKEN_SIGNING_KEY"];
            });

            services.AddScoped<Event>();

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                    .AddBasicAuthentication(options =>
                    {
                        options.EventsType = typeof(Event);
                    });

            return services;
        }

        
    }
}
