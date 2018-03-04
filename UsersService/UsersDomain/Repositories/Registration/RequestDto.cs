using System;
using System.Collections.Generic;
using System.Text;

namespace UsersDomain.Repositories.Registration
{
    public class RequestDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public TimeSpan TTL { get; set; }
    }
}
