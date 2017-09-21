using Microsoft.AspNetCore.Authentication;
using System;
using UsersService.Authentication.Basic;

namespace UsersService.Extensions
{
    public static class AuthenticationBuilderExtension
    {
        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, Action<BasicAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicDefaults.AuthenticationScheme, BasicDefaults.AuthenticationScheme, configureOptions);
        }
    }
}
