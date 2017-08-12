using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StorageDomain.Repositories;
using StorageDomain.Services;
using StorageService.Commands;
using StorageService.Events;
using StorageService.Options;
using StorageService.Queries;
using StorageService.Repositories;
using StorageService.Services;
using StorageService.Storages;
using System;

namespace StorageService.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection serviceCollection, string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("storage");
            return serviceCollection.AddSingleton(database);
        }

        public static IServiceCollection AddEvents(this IServiceCollection services, EventsConfiguration configuration)
        {
            services.Configure<EventsOptions>(options =>
            {
                options.MakeSnapshotForOlderThan = TimeSpan.FromSeconds(configuration.IsOldSeconds);
                options.StartSnapshotMakingLimit = configuration.MakeSnapshotLimit;
            });

            return services.AddTransient<IEventVisitorFactory, EventVisitorFactory>()
                           .AddTransient<IEventReaderCreator, EventReaderCreator>()
                           .AddTransient<IEventStore, EventStore>()
                           .AddTransient<ISnapshot, Snapshot>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services, ServicesConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<ApiOptions>(options =>
            {
                options.BaseUri = new Uri(configuration.ApiUrl);
            });

            services.Configure<SecurityOptions>(options =>
            {
                options.Login = configuration.Login;
                options.Password = configuration.Password;
            });

            return services.AddTransient<ISecurityService, SecurityService>()
                           .AddTransient<IUsersService, UsersService>()
                           .AddTransient<IMapService, MapService>();
        }

        public static IServiceCollection AddStorages(this IServiceCollection services)
        {
            return services.AddTransient<IStorageStore, StorageStore>();
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddTransient<IUserValidationService, UserValidationService>()
                           .AddTransient<ITransactionValidationService, TransactionValidationService>()
                           .AddTransient<IUsersRepository, UserRepository>()
                           .AddTransient<IMapRepository, MapRepository>();
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.AddTransient<ICommandFactory, CommandFactory>();
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.AddTransient<IQueryFactory, QueryFactory>();
        }
    }
}
