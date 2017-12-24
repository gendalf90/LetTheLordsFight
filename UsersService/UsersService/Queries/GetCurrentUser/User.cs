using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Queries.GetCurrentUser
{
    public class User
    {
        public string Login { get; set; }

        public string[] Roles { get; set; }
    }
}
