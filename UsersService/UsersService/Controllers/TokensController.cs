using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersService.Queries;

namespace UsersService.Controllers
{
    [Authorize]
    [Route("api/v1/users/current/token")]
    public class TokensController : Controller
    {
        private readonly IFactory queries;

        public TokensController(IFactory queries)
        {
            this.queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var query = queries.CreateGetTokenQuery();
            var result = await query.AskAsync();
            return Ok(result);
        }
    }
}
