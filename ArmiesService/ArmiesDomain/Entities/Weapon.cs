using ArmiesDomain.Repositories.Weapons;
using ArmiesDomain.Services;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Weapon
    {
        private Cost cost;
        private List<Offence> offence;
        private List<Tag> tags;

        private Weapon(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Weapon name is empty");
            }

            Name = name;
            cost = new Cost();
            offence = new List<Offence>();
            tags = new List<Tag>();
        }

        public string Name { get; private set; }

        public void ApplyService(IArmyCostLimit service)
        {
            service.AccumulateCost(cost);
        }

        public static async Task<Weapon> LoadAsync(IWeapons repository, string name)
        {
            var data = await repository.GetByNameAsync(name);
            var weapon = new Weapon(data.Name);
            weapon.cost = new Cost(data.Cost);
            weapon.offence = data.Offence
                                 .Select(LoadOffence)
                                 .ToList();
            weapon.tags = data.Tags
                              .Select(tag => new Tag(tag))
                              .ToList();

            return weapon;
        }

        private static Offence LoadOffence(OffenceDto data)
        {
            var range = new Range(data.Min, data.Max);
            var tags = data.Tags
                           .Select(tag => new Tag(tag))
                           .ToList();
            return new Offence(range, tags);
        }
    }
}
