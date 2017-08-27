using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapService.Queries;

namespace MapService.Controllers
{
    [Route("api/v1/map")]
    class MapController : Controller
    {
        private readonly IQueryFactory queryFactory;

        public MapController(IQueryFactory queryFactory)
        {
            this.queryFactory = queryFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetMap()
        {
            var query = queryFactory.CreateMapQuery();

            try
            {
                var result = await query.GetJsonAsync();
                return Json(result);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
