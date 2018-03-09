using System.Collections.Generic;

namespace UsersService.BasicAuthentication
{
    public class UserDto
    {
        public string Login { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
