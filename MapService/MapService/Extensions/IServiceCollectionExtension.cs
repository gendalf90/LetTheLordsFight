using Cassandra;
using Cassandra.Mapping;
using MapDomain.Common;
using MapDomain.Factories;
using MapDomain.Repositories;
using MapDomain.Services;
using MapService.Commands;
using MapService.Factories;
using MapService.Options;
using MapService.Queries;
using MapService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Extensions
{
    static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCassandra(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var cassandraConnectionString = configuration["CASSANDRA_CONNECTION_STRING"];
            var cassandraConnectionData = cassandraConnectionString.Split(';');
            var nodes = cassandraConnectionData.Reverse().Skip(2).ToArray();
            var login = cassandraConnectionData.Reverse().Skip(1).First();
            var password = cassandraConnectionData.Last();

            var cluster = Cluster.Builder()
                                 .AddContactPoints(nodes)
                                 .WithCredentials(login, password)
                                 .Build();
            var session = cluster.Connect("map");
            return serviceCollection.AddSingleton(session);
        }

        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            MappingConfiguration.Global.Define(new Map<MapObjectRepositoryData>().TableName("objects").PartitionKey(data => data.Id));

            services.Configure<MapOptions>(configuration);

            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                           .AddTransient<IMapFactory, MapFactory>()
                           .AddTransient<IMapObjectsRepository, MapObjectsRepository>()
                           .AddTransient<IUsersRepository, UsersRepository>()
                           .AddTransient<IUserValidationService, UserValidationService>();
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
