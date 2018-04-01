using System;

namespace UsersService.Database
{
    class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public RoleDto[] Roles { get; set; }
    }
}
