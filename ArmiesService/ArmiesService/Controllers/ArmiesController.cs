using ArmiesService.Commands;
using ArmiesService.Controllers.Data;
using ArmiesService.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AggregateExceptionExtensions.Handler;
using ArmiesDomain.Exceptions;

namespace ArmiesService.Controllers
{
    [Authorize]
    [Route("api/v1/armies")]
    public class ArmiesController : Controller
    {
        private readonly IFactory commands;
        private readonly ILog log;

        public ArmiesController(IFactory commands, ILog log)
        {
            this.commands = commands;
            this.log = log;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ArmyDto data)
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

        private async Task TryCreateArmyAsync(ArmyDto data)
        {
            try
            {
                var command = commands.GetCreateArmyCommand(data);
                await command.ExecuteAsync();
            }
            catch (AggregateException ae)
            {
                ae.Flatten()
                  .AddHandlers()
                  .Action<EntityNotFoundException>(HandleEntityNotFoundException)
                  .Action<QuantityException>(HandleQuantityException)
                  .Predicate<SquadException>(TryHandleSquadException)
                  .Predicate<ArmyException>(TryHandleArmyException)
                  .Handle();
            }
        }

        private void HandleEntityNotFoundException(EntityNotFoundException e)
        {
            HandleValidationError("entityName", e.Message);
        }

        private void HandleQuantityException(QuantityException e)
        {
            HandleValidationError("squadQuantity", e.Message);
        }

        private bool TryHandleSquadException(SquadException e)
        {
            if(e.ParamName != "quantity")
            {
                return false;
            }

            HandleValidationError("squadQuantity", e.Message);
            return true;
        }

        private bool TryHandleArmyException(ArmyException e)
        {
            if(e.ParamName != "squads")
            {
                return false;
            }

            HandleValidationError("armySquads", e.Message);
            return true;
        }

        private void HandleValidationError(string parameter, string description)
        {
            ModelState.AddModelError("validation", parameter);
            log.Warning($"Validation error '{description}' from user '{User.Identity.Name}' request");
        }
    }
}
