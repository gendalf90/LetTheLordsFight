using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
