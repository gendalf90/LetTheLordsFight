using System;
using System.Threading.Tasks;

namespace UsersDomain.Repositories.Registration
{
    public interface IRequests
    {
        Task SaveAsync(RequestDto data); //unique index on login field and throw exception if exist!

        Task<RequestDto> GetByIdAsync(Guid id); //throw RequestException if not found
    }
}
