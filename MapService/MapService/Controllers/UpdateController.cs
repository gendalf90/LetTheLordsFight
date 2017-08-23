using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MapService.Controllers
{
    [Route("api/v1/map/elapsed")]
    public class UpdateController : Controller
    {
        [HttpPost("{elapsedSeconds}")]
        public IActionResult Update(float elapsedSeconds)
        {
            //linq.asparallel.forall on bucket

            return Json(TimeSpan.FromSeconds(elapsedSeconds));
        }
    }
}
