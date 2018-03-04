using System;
using System.Collections.Generic;
using System.Text;
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

        public void Save(IRequests repository)
        {

        }
    }
}
