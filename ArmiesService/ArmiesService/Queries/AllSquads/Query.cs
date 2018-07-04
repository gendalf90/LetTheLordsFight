using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllSquads
{
    public class Query : IQuery<IEnumerable<SquadDto>>
    {
        public Task<IEnumerable<SquadDto>> AskAsync()
        {
            throw new NotImplementedException();
        }
    }
}
