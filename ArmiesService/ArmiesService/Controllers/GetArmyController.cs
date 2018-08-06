using ArmiesService.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArmiesService.Controllers
{
    [Authorize]
    [Route("api/v1/armies")]
    public class GetArmyController : Controller
    {
        private readonly IQueriesFactory queries;

        public GetArmyController(IQueriesFactory queries)
        {
            this.queries = queries;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyArmyAsync()
        {
            var query = queries.CreateArmyQuery();
            var result = await query.AskAsync();

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
