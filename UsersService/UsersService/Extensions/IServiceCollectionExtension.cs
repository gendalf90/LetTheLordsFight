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
using Microsoft.EntityFrameworkCore;
using UsersService.Database;
using UsersDomain.Repositories.Registration;

namespace UsersService.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMySql(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration["MYSQL_CONNECTION_STRING"];
            return serviceCollection.AddDbContext<Context>(options => options.UseMySql(connectionString));
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                           .AddTransient<IGetTokenSigningKeyStrategy, GetTokenSigningKeyStrategy>()
                           .AddTransient<IGetCurrentUserStrategy, GetCurrentUserStrategy>()
                           .AddTransient<IQueryFactory, QueryFactory>();
        }

        public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<ConfirmationOptions>(options =>
                           {
                               options.Link = configuration["CONFIRMATION_LINK"];
                           })
                           .AddTransient<ICommandFactory, CommandFactory>()
                           .AddTransient<IConfirmationLink, ConfirmationLink>();
        }

        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<SmtpOptions>(options =>
                           {
                               options.Host = configuration["SMTP_HOST"];
                               options.Port = int.Parse(configuration["SMTP_PORT"]);
                               options.Login = configuration["SMTP_LOGIN"];
                               options.Password = configuration["SMTP_PASSWORD"];
                           })
                           .AddTransient<IRequests, RequestsRepository>()
                           .AddTransient<IUsers, UsersRepository>()
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
                    })
                    .AddScoped<Event>()
                    .AddTransient<IGetUserByLoginAndPasswordStrategy, GetUserByLoginAndPasswordStrategy>()
                    .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                    .AddBasicAuthentication(options =>
                    {
                        options.EventsType = typeof(Event);
                    });

            return services;
        }

        
    }
}
