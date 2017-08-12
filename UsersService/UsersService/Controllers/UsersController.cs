using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using UsersService.Common;

namespace UsersService.Controllers
{
    [Route("api/v1/users")]
    public class UsersController : Controller
    {
        [Authorize(ActiveAuthenticationSchemes = "Basic")]
        [HttpPost("current/token")]
        public ActionResult CreateToken()
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = User.Claims.ToList();

            //claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var jwt = new JwtSecurityToken(signingCredentials: jwtSettings.Sign,
                                           //issuer: jwtSettings.Issuer,
                                           //audience: jwtSettings.Audience,
                                           //expires: DateTime.UtcNow.Add(jwtSettings.ValidTime),
                                           claims: claims);

            return Json(new { Token = handler.WriteToken(jwt)/*, Expires = jwtSettings.ValidTime.TotalSeconds */});
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet("current")]
        public IActionResult GetUser()
        {
            return NotFound();
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult GetUserByParameter([FromQuery] FindByData parameters)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            


            return NotFound();
        }

        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            

            return NotFound();
        }
    }
}
