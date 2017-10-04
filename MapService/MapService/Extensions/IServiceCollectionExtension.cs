using MapDomain.Factories;
using MapDomain.Repositories;
using MapDomain.Services;
using MapService.Commands;
using MapService.Factories;
using MapService.Options;
using MapService.Queries;
using MapService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace MapService.Extensions
{
    static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connection = configuration["MONGO_CONNECTION_STRING"];
            var client = new MongoClient(connection);
            var database = client.GetDatabase("map");
            return serviceCollection.AddSingleton(database);
        }

        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
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
