using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MapViewService.Controllers
{
    [Route("map")]
    public class HomeController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
