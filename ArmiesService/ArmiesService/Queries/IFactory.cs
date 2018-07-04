using ArmiesService.Queries.Army;
using SquadDescriptionDto = ArmiesService.Queries.AllSquads.SquadDto;
using WeaponDescriptionDto = ArmiesService.Queries.AllWeapons.WeaponDto;
using ArmorDescriptionDto = ArmiesService.Queries.AllArmors.ArmorDto;
using System.Collections.Generic;

namespace ArmiesService.Queries
{
    public interface IFactory
    {
        IQuery<ArmyDto> CreateArmyQuery();

        IQuery<IEnumerable<SquadDescriptionDto>> CreateAllSquadsQuery();

        IQuery<IEnumerable<WeaponDescriptionDto>> CreateAllWeaponsQuery();

        IQuery<IEnumerable<ArmorDescriptionDto>> CreateAllArmorsQuery();
    }
}
