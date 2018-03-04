using System;
using System.Collections.Generic;
using System.Text;
using UsersDomain.Services.Registration;
using UsersDomain.ValueTypes.Confirmation;

namespace UsersDomain.ValueTypes.Registration
{
    public class Email
    {
        public Email(Login login, Password password, Link confirmationLink, TTL ttl)
        {

        }

        public void Send(IEmail service)
        {
            //body in dto: (login + password + request confirm PAGE path)
        }
    }
}
