using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Common;
using MapDomain.Exceptions;
using MapService.Queries;
using MapService.Commands;
using Microsoft.AspNetCore.Authorization;

namespace MapService.Controllers
{
    [Authorize]
    [Route("api/v1/map/objects")]
    class ObjectsController : Controller
    {
        private readonly IQueryFactory queryFactory;
        private readonly ICommandFactory commandFactory;

        public ObjectsController(IQueryFactory queryFactory, ICommandFactory commandFactory)
        {
            this.queryFactory = queryFactory;
            this.commandFactory = commandFactory;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddObjectAsync(string id, [FromBody] MapObjectCreateData createData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var command = commandFactory.GetAddMapObjectCommand(id, createData);

            try
            {
                await command.ExecuteAsync();
            }
            catch(NoPermissionException)
            {
                return Forbid();
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetObjectAsync(string id)
        {
            var query = queryFactory.CreateObjectQuery(id);

            try
            {
                return Json(await query.GetJsonAsync());
            }
            catch(NoPermissionException)
            {
                return Forbid();
            }
            catch (InvalidOperationException e) when (e.Message == "Sequence contains no elements")
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateObject(string id, [FromBody] MapObjectUpdateData updateData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var command = commandFactory.GetUpdateMapObjectCommand(id, updateData);

            try
            {
                await command.ExecuteAsync();
            }
            catch (NoPermissionException)
            {
                return Forbid();
            }
            catch (InvalidOperationException e) when (e.Message == "Sequence contains no elements")
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
