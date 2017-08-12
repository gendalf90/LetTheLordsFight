using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public interface IQuery
    {
        Task<string> GetJsonAsync();
    }
}
