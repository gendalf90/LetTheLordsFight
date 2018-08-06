using ArmiesService.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArmiesService.Controllers
{
    [Authorize]
    [Route("api/v1/armies/aboutme")]
    public class AboutMeController : Controller
    {
        private readonly IQueriesFactory queries;

        public AboutMeController(IQueriesFactory queries)
        {
            this.queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDataAsync()
        {
            var query = queries.CreateUserQuery();
            var result = await query.AskAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
