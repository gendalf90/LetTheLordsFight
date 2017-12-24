using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Queries.GetUserByLogin
{
    public class User
    {
        public string Login { get; set; }

        public string[] Roles { get; set; }
    }
}
