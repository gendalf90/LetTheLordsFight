using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using UsersService.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace UsersService.Controllers
{
    [Route("api/v1/users")]
    public class UsersController : Controller
    {
        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpPost("current/token")]
        public IActionResult CreateToken()
        {
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "system")]
        [HttpGet("{login}")]
        public IActionResult GetByLogin(string login)
        {
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{login}/roles")]
        public IActionResult GetRoles(string login)
        {
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{login}/roles/{role}")]
        public IActionResult AddRole(string login, string role)
        {
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{login}/roles/{role}")]
        public IActionResult DeleteRole(string login, string role)
        {
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "system")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            

            return NotFound();
        }
    }
}
