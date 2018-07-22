using System.Threading.Tasks;

namespace ArmiesService.Common.CachingOperations
{
    interface IInsertEntityStrategy<T> where T : class
    {
        Task InsertAsync(T entity);
    }
}
