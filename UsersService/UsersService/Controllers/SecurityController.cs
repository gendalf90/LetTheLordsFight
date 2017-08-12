using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersService.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UsersService.Controllers
{
    [Route("api/v1/users/current")]
    public class SecurityController : Controller
    {
        private JwtOptions jwtSettings;

        public SecurityController(IOptions<JwtOptions> jwtOptions)
        {
            jwtSettings = jwtOptions.Value;
        }

        [Authorize(ActiveAuthenticationSchemes = "Basic")]
        [HttpGet("token")]
        public ActionResult GetToken()
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = User.Claims.ToList();
            
            //claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var jwt = new JwtSecurityToken(signingCredentials: jwtSettings.Sign,
                                           //issuer: jwtSettings.Issuer,
                                           //audience: jwtSettings.Audience,
                                           //expires: DateTime.UtcNow.Add(jwtSettings.ValidTime),
                                           claims: claims);
            
            return Ok(new { Token = handler.WriteToken(jwt)/*, Expires = jwtSettings.ValidTime.TotalSeconds */});
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet]
        public ActionResult GetByToken()
        {
            var login = User.FindFirst("login").Value;
            return Json(new { Login = login, Type = "SimpleUser" });
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet("authentication/result")]
        public ActionResult GetAuthorizationResult() => Ok();
    }
}
