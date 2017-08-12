using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseTokens(this IApplicationBuilder builder, string tokenSigningKey)
        {
            return builder.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = CreateSecurityKey(tokenSigningKey),
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
