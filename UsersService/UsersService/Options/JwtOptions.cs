using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Options
{
    public class JwtOptions
    {
        //public string Issuer { get; set; }
        //public string Audience { get; set; }
        //public TimeSpan ValidTime { get; set; }
        public SigningCredentials Sign { get; set; }
    }
}
