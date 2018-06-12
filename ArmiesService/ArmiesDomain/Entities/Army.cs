using ArmiesDomain.Factories.Armies;
using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Services.Costs;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Army
    {
        private List<Squad> squads;

        public Army(string owner, IEnumerable<Squad> squads)
        {
            if(string.IsNullOrEmpty(owner))
            {
                throw new ArgumentException("Owner must be set");
            }

            if(squads == null || !squads.Any())
            {
                throw new ArgumentException("Squads list is empty");
            }

            Owner = owner;
            this.squads = squads.ToList();
        }

        public string Owner { get; private set; }

        public void ApplyCostService(ICost service)
        {
            foreach(var squad in squads)
            {
                squad.ApplyCostService(service);
            }
        }

        public async Task SaveAsync(IArmies repository)
        {
            var data = new ArmyDto();
            data.Owner = Owner;
            squads.ForEach(squad => squad.FillArmyData(data));
            await repository.SaveAsync(data);
        }
    }
}
