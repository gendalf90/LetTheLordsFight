using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Queries
{
    public interface IQuery<T>
    {
        Task<T> AskAsync();
    }
}
