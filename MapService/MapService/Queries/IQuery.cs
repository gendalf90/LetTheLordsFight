using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public interface IQuery
    {
        Task<JObject> GetJsonAsync();
    }
}
