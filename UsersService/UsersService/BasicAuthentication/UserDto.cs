using System.Collections.Generic;

namespace UsersService.BasicAuthentication
{
    public class UserDto
    {
        public string Login { get; set; }

        public string[] Roles { get; set; }
    }
}
