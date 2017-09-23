using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Queries
{
    interface IQueryFactory
    {
        IQuery CreateLoginQuery(string login);

        IQuery CreateCurrentQuery();

        IQuery CreateTokenQuery();
    }
}
