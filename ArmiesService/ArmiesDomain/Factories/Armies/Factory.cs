using ArmiesDomain.Entities;
using ArmiesDomain.Repositories.Armors;
using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Repositories.Users;
using ArmiesDomain.Repositories.Weapons;
using ArmiesDomain.Services;
using ArmiesDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesDomain.Factories.Armies
{
    public class Factory : IFactory
    {
        private readonly ISquads squads;
        private readonly IWeapons weapons;
        private readonly IArmors armors;
        private readonly IUsers users;
        private readonly IArmyCostLimit costLimit;

        public Factory(ISquads squads, 
                       IWeapons weapons, 
                       IArmors armors, 
                       IUsers users,
                       IArmyCostLimit costLimit)
        {
            this.squads = squads;
            this.weapons = weapons;
            this.armors = armors;
            this.users = users;
            this.costLimit = costLimit;
        }

        public async Task<Army> BuildAsync(ArmyData data)
        {
            var owner = await GetOwnerAsync(data);
            var squads = await BuildSquadsAsync(data);
            var army = new Army(owner.Login, squads);
            army.ApplyService(costLimit);
            costLimit.CheckForUser(owner);
            return army;
        }

        private async Task<User> GetOwnerAsync(ArmyData data)
        {
            return await User.LoadByLoginAsync(users, data.OwnerLogin);
        }

        private async Task<IEnumerable<Squad>> BuildSquadsAsync(ArmyData data)
        {
            if(data.Squads == null || !data.Squads.Any())
            {
                return Enumerable.Empty<Squad>();
            }

            return await Task.WhenAll(data.Squads.Select(BuildSquadAsync));
        }

        private async Task<Squad> BuildSquadAsync(SquadData data)
        {
            var quantity = new Quantity(data.Quantity);
            var weapons = await GetWeaponsAsync(data);
            var armors = await GetArmorsAsync(data);
            var squad = await GetSquadAsync(data);

            squad.SetQuantity(quantity);

            foreach(var weapon in weapons)
            {
                squad.AddWeapon(weapon);
            }

            foreach(var armor in armors)
            {
                squad.AddArmor(armor);
            }

            return squad;
        }

        private async Task<Squad> GetSquadAsync(SquadData squadData)
        {
            return await Squad.LoadAsync(squads, squadData.Type);
        }

        private async Task<IEnumerable<Weapon>> GetWeaponsAsync(SquadData squadData)
        {
            if(squadData.Weapons == null || !squadData.Weapons.Any())
            {
                return Enumerable.Empty<Weapon>();
            }

            return await Task.WhenAll(squadData.Weapons.Select(GetWeaponAsync));
        }

        private async Task<Weapon> GetWeaponAsync(string name)
        {
            return await Weapon.LoadAsync(weapons, name);
        }

        private async Task<IEnumerable<Armor>> GetArmorsAsync(SquadData squadData)
        {
            if (squadData.Armors == null || !squadData.Armors.Any())
            {
                return Enumerable.Empty<Armor>();
            }

            return await Task.WhenAll(squadData.Armors.Select(GetArmorAsync));
        }

        private async Task<Armor> GetArmorAsync(string name)
        {
            return await Armor.LoadAsync(armors, name);
        }
    }
}
