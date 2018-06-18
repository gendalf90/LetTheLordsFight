using ArmiesDomain.Repositories.Armies;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Services;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SquadDtoOfArmyRepository = ArmiesDomain.Repositories.Armies.SquadDto;

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
                throw new ArgumentException("Squad type is empty");
            }

            Type = type;
            cost = new Cost();
            tags = new List<Tag>();
            quantity = new Quantity();
            weapons = new List<Weapon>();
            armors = new List<Armor>();
        }

        public string Type { get; private set; }

        public void ApplyService(IArmyCostLimit service)
        {
            foreach(var weapon in weapons)
            {
                weapon.ApplyService(service);
            }

            service.AccumulateCost(cost);
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
            this.quantity = quantity;
        }

        public void FillArmyData(ArmyDto armyData)
        {
            var squadData = new SquadDtoOfArmyRepository();
            squadData.Type = Type;
            squadData.Weapons = weapons.Select(weapon => weapon.Name)
                                       .ToList();
            squadData.Armors = armors.Select(armor => armor.Name)
                                     .ToList();
            quantity.FillSquadData(squadData);
            cost.FillSquadData(squadData);
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
