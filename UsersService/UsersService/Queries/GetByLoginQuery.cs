using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using UsersService.Options;
using MySql.Data.MySqlClient;
using Dapper;

namespace UsersService.Queries
{
    class GetByLoginQuery : IQuery
    {
        private readonly string login;
        private readonly IOptions<SqlOptions> sqlOptions;

        private IEnumerable<Roles> roles;

        public GetByLoginQuery(IOptions<SqlOptions> sqlOptions, string login)
        {
            this.login = login;
            this.sqlOptions = sqlOptions;
        }

        public async Task<string> AskAsync()
        {
            await LoadRolesAsync();
            return CreateJsonResult();
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

        private string CreateJsonResult()
        {
            var result = new
            {
                Login = login,
                Roles = roles.Select(role => role.Role)
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
