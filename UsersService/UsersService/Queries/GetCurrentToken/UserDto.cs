using System.Collections.Generic;

namespace UsersService.Queries.GetCurrentToken
{
    public class UserDto
    {
        public string Login { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
