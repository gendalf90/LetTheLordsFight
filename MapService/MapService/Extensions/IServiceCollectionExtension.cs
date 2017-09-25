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
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                           .AddSingleton<IMapFactory, MapFactory>()
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

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSigningKey = CreateTokenSigningKey(configuration);

            services.AddAuthentication().AddJwtBearer(options =>
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
