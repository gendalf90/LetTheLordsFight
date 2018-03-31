using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Database
{
    [Table("users")]
    class UserDto
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        public ICollection<RoleDto> Roles { get; set; }
    }
}
