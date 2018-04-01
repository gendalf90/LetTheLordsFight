using System;
using System.Collections.Generic;

namespace UsersService.Database
{
    class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
}
