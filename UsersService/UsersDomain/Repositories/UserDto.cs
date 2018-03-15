using System;
using UsersDomain.ValueTypes;

namespace UsersDomain.Repositories
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Role[] Roles { get; set; }
    }
}
