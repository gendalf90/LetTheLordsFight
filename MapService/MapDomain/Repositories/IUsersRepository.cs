using MapDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Repositories
{
    public interface IUsersRepository
    {
        User GetCurrent();
    }
}
