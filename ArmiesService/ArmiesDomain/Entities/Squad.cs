using ArmiesDomain.Exceptions;
using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Services;
using ArmiesDomain.Services.ArmyNotifications;
using ArmiesDomain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SquadDtoOfArmyRepository = ArmiesDomain.Repositories.Armies.SquadRepositoryDto;

namespace ArmiesDomain.Entities
{
    public class Squad
    {
        private Quantity quantity;
        private Cost cost;
        private List<Tag> tags;
        private List<Weapon> weapons;
        private List<Armor> armors;

        private Squad(string type)
        {
            if(string.IsNullOrEmpty(type))
            {
                throw SquadException.CreateType();
            }

            Type = type;
            cost = new Cost();
            tags = new List<Tag>();
            quantity = Quantity.Single;
            weapons = new List<Weapon>();
            armors = new List<Armor>();
        }

        public string Type { get; private set; }

        public void CheckCostLimit(IArmyCostLimitService service)
        {
            foreach(var weapon in weapons)
            {
                weapon.CheckCostLimit(service);
            }

            foreach(var armor in armors)
            {
                armor.CheckCostLimit(service);
            }

            var costRelatedOnQuantity = quantity.Multiply(cost);
            service.AccumulateCost(costRelatedOnQuantity);
        }

        public void AddWeapon(Weapon weapon)
        {
            weapons.Add(weapon);
        }

        public void AddArmor(Armor armor)
        {
            armors.Add(armor);
        }

        public void SetQuantity(Quantity quantity)
        {
            if(quantity.IsZero)
            {
                throw SquadException.CreateQuantity();
            }

            this.quantity = quantity;
        }

        public void FillArmyData(ArmyRepositoryDto armyData)
        {
            var squadData = new SquadDtoOfArmyRepository();
            squadData.Type = Type;
            squadData.Weapons = weapons.Select(weapon => weapon.Name)
                                       .ToList();
            squadData.Armors = armors.Select(armor => armor.Name)
                                     .ToList();
            quantity.FillSquadData(squadData);
            armyData.Squads.Add(squadData);
        }

        public void FillArmyData(ArmyNotificationDto armyData)
        {
            var squadData = new SquadNotificationDto
            {
                Weapons = new List<WeaponNotificationDto>(),
                Armors = new List<ArmorNotificationDto>(),
                Tags = new List<string>()
            };

            squadData.Type = Type;
            weapons.ForEach(weapon => weapon.FillSquadData(squadData));
            armors.ForEach(armor => armor.FillSquadData(squadData));
            quantity.FillSquadData(squadData);
            tags.ForEach(tag => tag.FillSquadData(squadData));
            armyData.Squads.Add(squadData);
        }

        public static async Task<Squad> LoadAsync(ISquads repository, string type)
        {
            var data = await repository.GetByTypeAsync(type);
            var squad = new Squad(data.Type);
            squad.cost = new Cost(data.Cost);
            squad.tags = data.Tags
                             .Select(tag => new Tag(tag))
                             .ToList();

            return squad;
        }
    }
}
