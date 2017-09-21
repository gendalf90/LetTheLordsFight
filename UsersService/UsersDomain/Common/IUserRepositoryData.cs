using System;
using System.Collections.Generic;
using System.Text;

namespace UsersDomain.Common
{
    public interface IUserRepositoryData
    {
        string Login { get; set; }

        string Password { get; set; }

        IEnumerable<string> Roles { get; set; }
    }
}
