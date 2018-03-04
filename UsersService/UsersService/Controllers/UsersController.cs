using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UsersService.Common;
using UsersService.Commands;
using UsersDomain.Exceptions;
using IQueryFactory = UsersService.Queries.IFactory;
using System;

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
            var query = queries.CreateGetTokenQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }

        [HttpPost("registration/request")]
        public async Task<IActionResult> CreateRegistrationRequestAsync([FromBody] RegistrationData data)
        {
            throw new NotImplementedException();
        }

        [HttpPost("confirmation/request/{id:guid}")]
        public async Task<IActionResult> CreateUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

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
            catch(LoginException e)
            {
                ModelState.AddModelError("login", e.Message);
            }
            catch(PasswordException e)
            {
                ModelState.AddModelError("password", e.Message);
            }
            catch(UserAlreadyExistException e)
            {
                ModelState.AddModelError("login", e.Message);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
