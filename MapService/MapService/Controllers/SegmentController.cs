using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MapService.Queries;
using MapDomain.Exceptions;

namespace MapService.Controllers
{
    [Route("api/v1/map/segment")]
    public class SegmentController : Controller
    {
        private readonly IQueryFactory queryFactory;

        public SegmentController(IQueryFactory queryFactory)
        {
            this.queryFactory = queryFactory;
        }

        [HttpGet("i/{i:int}/j/{j:int}")]
        public async Task<IActionResult> GetSegmentIJAsync(int i, int j)
        {
            var query = queryFactory.CreateSegmentQuery(i, j);

            try
            {
                var result = await query.GetJsonAsync();
                return Json(result);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("x/{x:float}/y/{y:float}")]
        public async Task<IActionResult> GetSegmentXYAsync(float x, float y)
        {
            var query = queryFactory.CreateSegmentQuery(x, y);

            try
            {
                var result = await query.GetJsonAsync();
                return Json(result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
