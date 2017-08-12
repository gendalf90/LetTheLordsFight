using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Common;
using MapDomain.Exceptions;
using MapService.Queries;

namespace MapService.Controllers
{
    [Route("api/v1/map/objects")]
    public class ObjectsController : Controller
    {
        private readonly IQueryFactory queryFactory;

        public ObjectsController(IQueryFactory queryFactory)
        {
            this.queryFactory = queryFactory;
        }

        [HttpPost]
        public IActionResult AddObject([FromBody] MapObjectCreateData createData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Created(Url.Action("GetObject", new { id = 2 }), new { Id = 2,
                                                                          X = createData.Position.X,
                                                                          Y = createData.Position.Y });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetObject(string id)
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
        public IActionResult UpdateObject(string id, [FromBody] MapObjectUpdateData updateData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
