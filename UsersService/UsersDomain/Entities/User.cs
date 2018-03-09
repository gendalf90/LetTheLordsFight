using System;
using System.Threading.Tasks;
using UsersDomain.Repositories;
using UsersDomain.ValueTypes;

namespace UsersDomain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public Task SaveAsync(IUsers repository)
        {
            throw new NotImplementedException();
        }

        public static User CreateSimple(Login login, Password password)
        {
            throw new NotImplementedException();
        }
    }
}
