using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Commands;
using MapDomain.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MapService.Controllers
{
    [Authorize]
    [Route("api/v1/map/elapsed")]
    class UpdateController : Controller
    {
        private readonly ICommandFactory commandFactory;

        public UpdateController(ICommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        [HttpPost("{elapsedSeconds:float}")]
        public async Task<IActionResult> UpdateMapAsync(float elapsedSeconds)
        {
            var command = commandFactory.GetUpdateMapCommand(TimeSpan.FromSeconds(elapsedSeconds));

            try
            {
                await command.ExecuteAsync();
            }
            catch (NoPermissionException)
            {
                return Forbid();
            }

            return Ok();
        }
    }
}
