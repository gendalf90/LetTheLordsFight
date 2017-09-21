using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Authentication.Basic
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public BasicAuthenticationOptions()
        {
        }
    }
}
