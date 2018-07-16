using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Repositories.Armors;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Repositories.Users;
using ArmiesDomain.Repositories.Weapons;
using ArmiesService.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersRepository = ArmiesService.Domain.Repositories.Users;

namespace ArmiesService.Initialization
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSigningKey = GetTokenSigningKey(configuration);

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

        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration["REDIS_CONNECTION_STRING"];
            return services.AddDistributedRedisCache(options => options.Configuration = connection);
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            Armies.RegisterTypes();
            UsersRepository.RegisterTypes();
            Weapons.RegisterTypes();
            Armors.RegisterTypes();
            Squads.RegisterTypes();

            return services.AddTransient<IArmies, Armies>()
                           .AddTransient<IUsers, UsersRepository>()
                           .AddTransient<IWeapons, Weapons>()
                           .AddTransient<IArmors, Armors>()
                           .AddTransient<ISquads, Squads>();
        }

        private static SecurityKey GetTokenSigningKey(IConfiguration configuration)
        {
            var sign = configuration["TOKEN_SIGNING_KEY"];
            var bytes = Encoding.ASCII.GetBytes(sign);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
