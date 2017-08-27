using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapService.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseTokens(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var sign = configuration["TOKEN_SIGNING_KEY"];

            return builder.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = CreateSecurityKey(sign),
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = false,
                    ValidateLifetime = false
                }
            });
        }

        private static SymmetricSecurityKey CreateSecurityKey(string sign)
        {
            var bytes = Encoding.ASCII.GetBytes(sign);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
