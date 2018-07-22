using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Common.CachingOperations
{
    interface IGetAllStrategy<T> where T: class
    {
        Task<IEnumerable<T>> GetAsync();
    }
}
