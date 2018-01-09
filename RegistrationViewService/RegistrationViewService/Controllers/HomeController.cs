using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RegistrationViewService.Options;

namespace RegistrationViewService.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IOptions<ApiOptions> options;

        public HomeController(IOptions<ApiOptions> options)
        {
            this.options = options;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //ViewBag.Api = options.Value.BaseUri.AbsoluteUri;
            return View();
        }
    }
}
