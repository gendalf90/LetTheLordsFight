using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UsersService.Common;
using UsersService.Commands;
using UsersDomain.Exceptions;
using ICommandFactory = UsersService.Commands.IFactory;
using IQueryFactory = UsersService.Queries.IFactory;
using System;
using UsersDomain.Exceptions.Registration;

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

        [Authorize]
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = commands.GetCreateRegistrationRequestCommand(data);

            try
            {
                await command.ExecuteAsync();
            }
            catch (LoginException e)
            {
                ModelState.AddModelError("login", e.Message);
            }
            catch (PasswordException e)
            {
                ModelState.AddModelError("password", e.Message);
            }
            catch(RequestException e)
            {
                ModelState.AddModelError("request", e.Message);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost("confirmation/request/{requestId:guid}")]
        public async Task<IActionResult> RegisterUserAsync(Guid requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = commands.GetRegisterUserCommand(requestId);

            try
            {
                await command.ExecuteAsync();
            }
            catch (RequestException e)
            {
                ModelState.AddModelError("request", e.Message);
            }
            catch (UserException e)
            {
                ModelState.AddModelError("user", e.Message);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
