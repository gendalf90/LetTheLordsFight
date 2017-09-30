using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using StorageDomain.Repositories;
using StorageDomain.Services;
using StorageService.Commands;
using StorageService.Events;
using StorageService.Options;
using StorageService.Queries;
using StorageService.Services;
using StorageService.Storages;
using System;
using System.Text;

namespace StorageService.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connection = configuration["MONGO_CONNECTION_STRING"];
            var client = new MongoClient(connection);
            var database = client.GetDatabase("storage");
            return serviceCollection.AddSingleton(database);
        }

        public static IServiceCollection AddEvents(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventsOptions>(options =>
            {
                options.MakeSnapshotForOlderThan = TimeSpan.FromSeconds(configuration.GetValue<int>("Events:IsOldSeconds"));
                options.StartSnapshotMakingLimit = configuration.GetValue<int>("Events:MakeSnapshotLimit");
            });

            services.AddSingleton<ISystemClock, SystemClock>();

            return services.AddTransient<IEventVisitorFactory, EventVisitorFactory>()
                           .AddTransient<IEventReaderCreator, EventReaderCreator>()
                           .AddTransient<IEventStore, EventStore>()
                           .AddTransient<ISnapshot, Snapshot>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<ApiOptions>(options =>
            {
                options.BaseUri = new Uri(configuration["API_URL"]);
            });

            services.Configure<SecurityOptions>(options =>
            {
                options.Login = configuration["STORAGE_SERVICE_LOGIN"];
                options.Password = configuration["STORAGE_SERVICE_PASSWORD"];
            });

            return services.AddTransient<ISecurityService, SecurityService>();
        }

        public static IServiceCollection AddStorages(this IServiceCollection services)
        {
            return services.AddTransient<IStorageStore, StorageStore>();
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddTransient<IUserService, UserService>()
                           .AddTransient<IUserValidationService, UserValidationService>()
                           .AddTransient<ITransactionValidationService, TransactionValidationService>()
                           .AddTransient<IUsersRepository, UserRepository>()
                           .AddTransient<IMapRepository, MapRepository>()
                           .AddTransient<IDistanceService, DistanceService>();
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.AddTransient<ICommandFactory, CommandFactory>();
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.AddTransient<IQueryFactory, QueryFactory>();
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSigningKey = CreateTokenSigningKey(configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            RequireExpirationTime = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = tokenSigningKey
                        };
                    });

            return services;
        }

        private static SecurityKey CreateTokenSigningKey(IConfiguration configuration)
        {
            var sign = configuration["TOKEN_SIGNING_KEY"];
            var bytes = Encoding.ASCII.GetBytes(sign);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
