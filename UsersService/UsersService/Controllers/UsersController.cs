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
using UsersService.Commands;
using UsersService.Queries;
using UsersDomain.Exceptions;

namespace UsersService.Controllers
{
    [Route("api/v1/users")]
    public class UsersController : Controller
    {
        private readonly ICommandFactory commands;
        private readonly IQueryFactory queries;

        public UsersController(ICommandFactory commands, IQueryFactory queries)
        {
            this.commands = commands;
            this.queries = queries;
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [HttpGet("current/token")]
        public async Task<IActionResult> GetTokenAsync()
        {
            var query = queries.CreateTokenQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentAsync()
        {
            var query = queries.CreateCurrentQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "System")]
        [HttpGet("{login}")]
        public async Task<IActionResult> GetByLoginAsync(string login)
        {
            var query = queries.CreateLoginQuery(login);
            var result = await query.AskAsync();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "System")]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = commands.GetCreateUserCommand(data);

            try
            {
                await command.ExecuteAsync();
            }
            catch(LoginInvalidException)
            {
                return BadRequest(new { Login = new[] { "Validation error" } });
            }
            catch(PasswordInvalidException)
            {
                return BadRequest(new { Password = new[] { "Validation error" } });
            }
                
            return Ok();
        }
    }
}
