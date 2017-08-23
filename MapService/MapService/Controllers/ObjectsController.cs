using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Common;
using MapDomain.Exceptions;
using MapService.Queries;
using MapService.Commands;

namespace MapService.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> AddObjectAsync([FromBody] MapObjectCreateData createData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var id = Guid.NewGuid().ToString("N");
            var command = commandFactory.GetAddMapObjectCommand(id, createData);

            try
            {
                await command.ExecuteAsync();
            }
            catch
            {

            }

            return Created(Url.Action("GetObjectAsync", new { id = id }), null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetObjectAsync(string id)
        {
            var query = queryFactory.CreateObjectQuery(id);

            try
            {
                var result = await query.GetJsonAsync();
                return Json(result);
            }
            catch(NoPermissionException)
            {
                return Forbid();
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
            catch
            {

            }

            return Ok();
        }
    }
}
