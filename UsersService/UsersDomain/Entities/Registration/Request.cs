using System;
using System.Threading.Tasks;
using UsersDomain.Repositories.Registration;
using UsersDomain.ValueTypes;
using UsersDomain.ValueTypes.Registration;

namespace UsersDomain.Entities.Registration
{
    public class Request
    {
        public Request(Login login, Password password, TTL ttl)
        {

        }

        public Guid Id { get; private set; }

        public Task SaveAsync(IRequests repository)
        {
            throw new NotImplementedException();
        }

        public static Task<Request> LoadAsync(Guid id, IRequests repository)
        {
            throw new NotImplementedException();
        }

        public User CreateSimpleUser()
        {
            throw new NotImplementedException();
        }
    }
}
