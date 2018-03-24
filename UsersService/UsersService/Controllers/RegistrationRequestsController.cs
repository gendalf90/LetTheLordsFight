using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersDomain.Exceptions;
using UsersDomain.Exceptions.Registration;
using UsersService.Commands;
using UsersService.Common;
using UsersService.Logs;

namespace UsersService.Controllers
{
    [Route("api/v1/users/registration/requests")]
    public class RegistrationRequestsController : Controller
    {
        private readonly IFactory commands;
        private readonly ILog log;

        private ICommand createCommand;
        private string login;

        public RegistrationRequestsController(IFactory commands, ILog log)
        {
            this.commands = commands;
            this.log = log;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RegistrationData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InitializeRegistrationData(data);
            await TryExecuteCreateCommandAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        private void InitializeRegistrationData(RegistrationData data)
        {
            login = data.Login;
            createCommand = commands.GetCreateRegistrationRequestCommand(data);
        }

        private async Task TryExecuteCreateCommandAsync()
        {
            try
            {
                await createCommand.ExecuteAsync();
            }
            catch (LoginException e)
            {
                ModelState.AddModelError("login", e.Message);
                log.Warning(e.Message);
            }
            catch (PasswordException e)
            {
                ModelState.AddModelError("password", e.Message);
                log.Warning($"Login {login}: {e.Message}");
            }
            catch (RequestException e)
            {
                ModelState.AddModelError("request", e.Message);
                log.Warning($"Login {login}: {e.Message}");
            }
        }
    }
}