using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Repositories.Armors;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Repositories.Users;
using ArmiesDomain.Repositories.Weapons;
using ArmiesService.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Extensions.Caching.Disabled;
using ArmiesService.Consumers;
using RabbitMQ.Client;
using MongoDB.Driver;

namespace ArmiesService.Initialization
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var sign = configuration["TOKEN_SIGNING_KEY"];

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
                            IssuerSigningKey = CreateTokenSigningKey(sign)
                        };
                    });

            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["REDIS_CONNECTION_STRING"];
            var expirationSeconds = configuration.GetValue<double?>("DistributedCache:ExpirationSeconds");

            services.Configure<DistributedCacheEntryOptions>(options =>
            {
                if(expirationSeconds.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSeconds.Value);
                }
            });

            var isDisabled = configuration.GetValue<bool?>("DistributedCache:Disabled");

            if (isDisabled == true)
            {
                return services.AddDisabledDistributedCache();
            }
            else
            {
                return services.AddDistributedRedisCache(options => options.Configuration = connectionString);
            }
        }

        public static IServiceCollection AddQueue(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["RABBITMQ_CONNECTION_STRING"];
            var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
            var connection = factory.CreateConnection();
            return services.AddSingleton(connection);
        }

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            return services.AddHostedService<UserCreatedEvent>();
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionSring = configuration["MONGODB_CONNECTION_STRING"];
            var client = new MongoClient(connectionSring);
            var database = client.GetDatabase("armies");
            return services.AddSingleton(database);
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddTransient<IArmies, Armies>()
                           .AddTransient<IUsers, Users>()
                           .AddTransient<IWeapons, Weapons>()
                           .AddTransient<IArmors, Armors>()
                           .AddTransient<ISquads, Squads>();
        }

        private static SecurityKey CreateTokenSigningKey(string key)
        {
            var bytes = Encoding.ASCII.GetBytes(key);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
