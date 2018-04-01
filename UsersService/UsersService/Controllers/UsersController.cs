using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersDomain.Exceptions;
using System;
using UsersDomain.Exceptions.Registration;
using UsersService.Logs;
using UsersService.Commands;

namespace UsersService.Controllers
{
    [Route("api/v1/users/confirmation/request")]
    public class UsersController : Controller
    {
        private readonly IFactory commands;
        private readonly ILog log;

        private Guid requestId;
        private ICommand registerCommand;

        public UsersController(IFactory commands, ILog log)
        {
            this.commands = commands;
            this.log = log;
        }

        [HttpPost("{requestId:guid}")]
        public async Task<IActionResult> RegisterByRequestIdAsync(Guid requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InitializeRequestById(requestId);
            await TryExecuteRegisterCommandAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        private void InitializeRequestById(Guid guid)
        {
            requestId = guid;
            registerCommand = commands.GetRegisterUserCommand(guid);
        }

        private async Task TryExecuteRegisterCommandAsync()
        {
            try
            {
                await registerCommand.ExecuteAsync();
            }
            catch (RequestException e)
            {
                ModelState.AddModelError("request", e.Message);
                log.Warning(e.Message);
            }
            catch (UserException e)
            {
                ModelState.AddModelError("user", e.Message);
                log.Warning($"Request {requestId}: {e.Message}");
            }
        }
    }
}
