using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ArmiesService.Controllers
{
    [Authorize]
    [Route("api/v1/armies")]
    public class CreateArmyController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
