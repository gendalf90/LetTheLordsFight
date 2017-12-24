using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using UsersService.Options;
using MySql.Data.MySqlClient;
using Dapper;

namespace UsersService.Queries.GetUserByLogin
{
    class Query : IQuery<User>
    {
        private readonly string login;
        private readonly IOptions<SqlOptions> sqlOptions;

        private IEnumerable<Roles> roles;

        public Query(IOptions<SqlOptions> sqlOptions, string login)
        {
            this.login = login;
            this.sqlOptions = sqlOptions;
        }

        public async Task<User> AskAsync()
        {
            await LoadRolesAsync();
            return CreateResult();
        }

        private async Task LoadRolesAsync()
        {
            var request = new CommandDefinition(@"select r.role
                                                  from users u join roles r on u.id = r.userid
                                                  where u.login = @login", 
                                                  new { Login = login });

            using (var connection = new MySqlConnection(sqlOptions.Value.ConnectionString))
            {
                roles = await connection.QueryAsync<Roles>(request);
            }
        }

        private User CreateResult()
        {
            return new User
            {
                Login = login,
                Roles = roles.Select(role => role.Role).ToArray()
            };
        }
    }
}
