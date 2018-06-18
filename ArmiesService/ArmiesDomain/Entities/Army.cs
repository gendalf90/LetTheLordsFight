using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Army
    {
        private List<Squad> squads;

        public Army(string ownerLogin, IEnumerable<Squad> squads)
        {
            if(string.IsNullOrEmpty(ownerLogin))
            {
                throw new ArgumentException("Owner must be set");
            }

            if(squads == null || !squads.Any())
            {
                throw new ArgumentException("Squads list is empty");
            }

            OwnerLogin = ownerLogin;
            this.squads = squads.ToList();
        }

        public string OwnerLogin { get; private set; }

        public void ApplyService(IArmyCostLimit service)
        {
            foreach(var squad in squads)
            {
                squad.ApplyService(service);
            }
        }

        public async Task SaveAsync(IArmies repository)
        {
            var data = new ArmyDto();
            data.OwnerLogin = OwnerLogin;
            squads.ForEach(squad => squad.FillArmyData(data));
            await repository.SaveAsync(data);
        }
    }
}
