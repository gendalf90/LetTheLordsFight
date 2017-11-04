using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StorageViewService.Controllers
{
    [Route("storage")]
    public class HomeController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            ViewBag.Storage = id;
            ViewBag.Api = "http://localhost:25000";
            return View();
        }
    }
}
