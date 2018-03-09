using System;

namespace UsersDomain.Repositories
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
