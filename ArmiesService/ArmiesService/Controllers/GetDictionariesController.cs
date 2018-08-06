using ArmiesService.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArmiesService.Controllers
{
    [Route("api/v1/armies")]
    public class GetDictionariesController : Controller
    {
        private readonly IQueriesFactory queries;

        public GetDictionariesController(IQueriesFactory queries)
        {
            this.queries = queries;
        }

        [HttpGet("armors/all")]
        public async Task<IActionResult> GetAllArmorsAsync()
        {
            var query = queries.CreateAllArmorsQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }

        [HttpGet("weapons/all")]
        public async Task<IActionResult> GetAllWeaponsAsync()
        {
            var query = queries.CreateAllWeaponsQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }

        [HttpGet("squads/all")]
        public async Task<IActionResult> GetAllSquadsAsync()
        {
            var query = queries.CreateAllSquadsQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }
    }
}
