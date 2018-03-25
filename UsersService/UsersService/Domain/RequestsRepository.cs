using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersDomain.Repositories.Registration;

namespace UsersService.Domain
{
    public class RequestsRepository : IRequests
    {
        public Task<RequestDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(RequestDto data)
        {
            throw new NotImplementedException();
        }
    }
}
