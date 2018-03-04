using System;
using System.Collections.Generic;
using System.Text;

namespace UsersDomain.Services.Registration
{
    public interface IEmail
    {
        void Send(EmailDto data);
    }
}
