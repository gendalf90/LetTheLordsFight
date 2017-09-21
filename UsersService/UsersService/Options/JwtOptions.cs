using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Options
{
    public class JwtOptions
    {
        public SigningCredentials Sign { get; set; }
    }
}
