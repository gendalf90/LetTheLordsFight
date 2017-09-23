using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Commands;
using UsersService.Options;
using UsersService.Queries;
using UsersService.Users;

namespace UsersService.Extensions
{
    static class IServiceCollectionExtension
    {
        public static IServiceCollection AddMySql(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration["MYSQL_CONNECTION_STRING"];
            return serviceCollection.AddDbContext<UsersContext>(options => options.UseMySQL(connectionString));
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                           .AddTransient<IQueryFactory, QueryFactory>();
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            return services.AddTransient<IUsersStore, UsersStore>()
                           .AddTransient<ICommandFactory, CommandFactory>();
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSigningCredentials = CreateTokenSigningCredentials(configuration);

            services.Configure<JwtOptions>(settings =>
            {
                settings.Sign = tokenSigningCredentials;
            });

            services.AddAuthentication().AddBasic().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    RequireExpirationTime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenSigningCredentials.Key
                };
            });

            return services;
        }

        private static SigningCredentials CreateTokenSigningCredentials(IConfiguration configuration)
        {
            var sign = configuration["TOKEN_SIGNING_KEY"];
            var bytes = Encoding.ASCII.GetBytes(sign);
            var key = new SymmetricSecurityKey(bytes);
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
