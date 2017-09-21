using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Users
{
    [Table("roles")]
    class RoleData
    {
        public int Id { get; set; }

        [Column("userid")]
        public int UserDataId { get; set; }

        [Column("role")]
        public string Value { get; set; }
    }
}
