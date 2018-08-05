using ArmiesDomain.Repositories.Armors;
using ArmiesDomain.Services;
using ArmiesDomain.Services.ArmyNotifications;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesDomain.Entities
{
    public class Armor
    {
        private Cost cost;
        private List<Defense> defense;
        private List<Tag> tags;

        private Armor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Armor name is empty");
            }

            Name = name;
            cost = new Cost();
            defense = new List<Defense>();
            tags = new List<Tag>();
        }

        public string Name { get; private set; }

        public void CheckCostLimit(IArmyCostLimitService service)
        {
            service.AccumulateCost(cost);
        }

        public void FillSquadData(SquadNotificationDto data)
        {
            var armorDto = new ArmorNotificationDto
            {
                Defence = new List<DefenceNotificationDto>(),
                Tags = new List<string>()
            };

            armorDto.Name = Name;
            defense.ForEach(offence => offence.FillArmorData(armorDto));
            tags.ForEach(tag => tag.FillArmorData(armorDto));
            data.Armors.Add(armorDto);
        }

        public static async Task<Armor> LoadAsync(IArmors repository, string name)
        {
            var data = await repository.GetByNameAsync(name);
            var armor = new Armor(data.Name);
            armor.cost = new Cost(data.Cost);
            armor.defense = data.Defence
                                 .Select(LoadDefence)
                                 .ToList();
            armor.tags = data.Tags
                              .Select(tag => new Tag(tag))
                              .ToList();

            return armor;
        }

        private static Defense LoadDefence(DefenceRepositoryDto data)
        {
            var range = new Range(data.Min, data.Max);
            var tags = data.Tags
                           .Select(tag => new Tag(tag))
                           .ToList();
            return new Defense(range, tags);
        }
    }
}
