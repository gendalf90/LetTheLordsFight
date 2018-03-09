using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsersDomain.Services.Registration;
using UsersDomain.ValueTypes.Confirmation;

namespace UsersDomain.ValueTypes.Registration
{
    public class Email
    {
        public Email(Login login, Password password, Link confirmationLink, TTL ttl)
        {

        }

        public Task SendAsync(IEmail service)
        {
            //body in dto: (login + password + request confirm PAGE path)
            throw new NotImplementedException();
        }
    }
}
