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
    public class ArmyFactory : IArmyFactory
    {
        private readonly ISquads squads;
        private readonly IWeapons weapons;
        private readonly IArmors armors;
        private readonly IUsers users;
        private readonly IArmyCostLimitService costLimit;

        private ArmyFactoryDto armyData;
        private User armyOwner;
        private Squad[] armySquads;
        private Army army;

        public ArmyFactory(ISquads squads, 
                       IWeapons weapons, 
                       IArmors armors, 
                       IUsers users,
                       IArmyCostLimitService costLimit)
        {
            this.squads = squads;
            this.weapons = weapons;
            this.armors = armors;
            this.users = users;
            this.costLimit = costLimit;
        }

        public async Task<Army> BuildAsync(ArmyFactoryDto data)
        {
            SetArmyData(data);
            await CreateArmyAsync();
            CheckArmyCostLimit();
            return GetResult();
        }

        private void SetArmyData(ArmyFactoryDto data)
        {
            armyData = data ?? throw new ArgumentNullException($"Army factory data is null");
        }

        private async Task CreateArmyAsync()
        {
            await Task.WhenAll(LoadOwnerAsync(), BuildSquadsAsync());
            army = new Army(armyOwner.Login, armySquads);
        }

        private async Task LoadOwnerAsync()
        {
            armyOwner = await User.LoadByLoginAsync(users, armyData.OwnerLogin);
        }

        private async Task BuildSquadsAsync()
        {
            if(armyData.Squads == null || !armyData.Squads.Any())
            {
                armySquads = new Squad[0];
            }
            else
            {
                armySquads = await Task.WhenAll(armyData.Squads.Select(BuildSquadAsync));
            }
        }

        private async Task<Squad> BuildSquadAsync(SquadFactoryDto data)
        {
            var squadTask = GetSquadAsync(data);
            var weaponsTask = GetWeaponsAsync(data);
            var armorsTask = GetArmorsAsync(data);

            await Task.WhenAll(squadTask, weaponsTask, armorsTask);

            var squad = squadTask.Result;
            var quantity = new Quantity(data.Quantity);
            squad.SetQuantity(quantity);

            foreach (var weapon in weaponsTask.Result)
            {
                squad.AddWeapon(weapon);
            }

            foreach(var armor in armorsTask.Result)
            {
                squad.AddArmor(armor);
            }

            return squad;
        }

        private async Task<Squad> GetSquadAsync(SquadFactoryDto squadData)
        {
            return await Squad.LoadAsync(squads, squadData.Type);
        }

        private async Task<IEnumerable<Weapon>> GetWeaponsAsync(SquadFactoryDto squadData)
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

        private async Task<IEnumerable<Armor>> GetArmorsAsync(SquadFactoryDto squadData)
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

        private void CheckArmyCostLimit()
        {
            army.CheckCostLimit(costLimit);
            costLimit.CheckForUser(armyOwner);
        }

        private Army GetResult()
        {
            return army;
        }
    }
}
