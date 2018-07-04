using System;
using System.Threading.Tasks;

namespace ArmiesService.Queries.Army
{
    class Query : IQuery<ArmyDto>
    {
        public Task<ArmyDto> AskAsync()
        {
            throw new NotImplementedException();
        }
    }
}
