﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MapService.Queries;
using MapDomain.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MapService.Controllers
{
    [Authorize]
    [Route("api/v1/map/segment")]
    class SegmentController : Controller
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
                return Json(await query.GetJsonAsync());
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
                return Json(await query.GetJsonAsync());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
