using ArmiesService.Commands;
using ArmiesService.Controllers.Data;
using ArmiesService.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ArmiesDomain.Exceptions;

namespace ArmiesService.Controllers
{
    [Authorize]
    [Route("api/v1/armies")]
    public class ArmiesController : Controller
    {
        private readonly ICommandsFactory commands;
        private readonly ILog log;

        public ArmiesController(ICommandsFactory commands, ILog log)
        {
            this.commands = commands;
            this.log = log;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ArmyControllerDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await TryCreateArmyAsync(data);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        private async Task TryCreateArmyAsync(ArmyControllerDto data)
        {
            try
            {
                var command = commands.GetCreateArmyCommand(data);
                await command.ExecuteAsync();
            }
            catch(EntityNotFoundException e)
            {
                ModelState.AddModelError("validation", e.Message);
            }
            catch(QuantityException e)
            {
                ModelState.AddModelError("validation", e.Message);
            }
            catch(SquadException e)
            {
                if(!e.IsQuantity)
                {
                    throw;
                }

                ModelState.AddModelError("validation", e.Message);
            }
            catch(ArmyException e)
            {
                if (!e.IsSquads && !e.IsCost)
                {
                    throw;
                }

                ModelState.AddModelError("validation", e.Message);
            }
        }
    }
}
