using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UsersService.Common;
using UsersService.Commands;
using UsersDomain.Exceptions;
using IQueryFactory = UsersService.Queries.IFactory;

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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentAsync()
        {
            var query = queries.CreateGetCurrentUserQuery();
            var result = await query.AskAsync();
            return Json(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "System")]
        [HttpGet("{login}")]
        public async Task<IActionResult> GetByLoginAsync(string login)
        {
            var query = queries.CreateGetUserByLoginQuery(login);
            var result = await query.AskAsync();
            return Json(result);
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
            catch(UserTypeException e)
            {
                ModelState.AddModelError("type", e.Message);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
