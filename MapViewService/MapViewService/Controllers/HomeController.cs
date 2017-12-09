using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MapViewService.Options;

namespace MapViewService.Controllers
{
    [Route("map")]
    public class HomeController : Controller
    {
        private readonly IOptions<ApiOptions> options;

        public HomeController(IOptions<ApiOptions> options)
        {
            this.options = options;
        }

        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            ViewBag.Id = id;
            ViewBag.Api = options.Value.BaseUri.AbsoluteUri;
            return View();
        }
    }
}
