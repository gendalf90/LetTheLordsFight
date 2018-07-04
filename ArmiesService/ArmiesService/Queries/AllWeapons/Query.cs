using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllWeapons
{
    public class Query : IQuery<IEnumerable<WeaponDto>>
    {
        public Task<IEnumerable<WeaponDto>> AskAsync()
        {
            throw new NotImplementedException();
        }
    }
}
