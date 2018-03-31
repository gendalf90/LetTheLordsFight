using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersService.Database
{
    [Table("roles")]
    class RoleDto
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("userid")]
        public Guid UserId { get; set; }

        [Column("role")]
        public string Value { get; set; }
    }

}
