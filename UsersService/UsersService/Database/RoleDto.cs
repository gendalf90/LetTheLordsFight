using System;

namespace UsersService.Database
{
    class RoleDto
    {
        public Guid UserId { get; set; }

        public string Value { get; set; }

        public UserDto User { get; set; }
    }

}
