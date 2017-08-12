using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;

namespace StorageViewService.Controllers
{
    [Route("view/storage")]
    public class HomeController : Controller
    {
        [Route("{storageId}")]
        public IActionResult GetView(int storageId)
        {
            ViewData["storageId"] = $"{storageId}";
            ViewData["availableStorageIds"] = new int[] { 1 };
            return View("Index");
        }

        [Route("test")]
        public IActionResult Test([FromHeader] string authorization)
        {
            return Json(new[] { new { Id = 1, Type = "test", Name = authorization } });
        }
    }
}
