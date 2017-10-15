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
            return View();
        }
    }
}
