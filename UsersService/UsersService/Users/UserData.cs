using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Common;

namespace UsersService.Users
{
    [Table("users")]
    class UserData : IUserRepositoryData
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public IEnumerable<RoleData> Roles { get; set; }

        IEnumerable<string> IUserRepositoryData.Roles
        {
            get
            {
                return Roles?.Select(role => role.Value);
            }
            set
            {
                if (value == null)
                {
                    Roles = null;
                }
                else if(Roles != null)
                {
                    var existed = Roles.ToDictionary(role => role.Value, role => role);
                    Roles = value.Select(role => existed.ContainsKey(role) ? existed[role] : new RoleData { Value = role });
                }
                else
                {
                    Roles = value.Select(role => new RoleData { Value = role });
                }
            }
        }
    }
}
