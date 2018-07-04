using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllArmors
{
    public class Query : IQuery<IEnumerable<ArmorDto>>
    {
        public Task<IEnumerable<ArmorDto>> AskAsync()
        {
            throw new NotImplementedException();
        }
    }
}
