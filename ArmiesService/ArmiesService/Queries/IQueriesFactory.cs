using ArmiesService.Queries.Army;
using SquadDescriptionDto = ArmiesService.Queries.AllSquads.SquadQueryDto;
using WeaponDescriptionDto = ArmiesService.Queries.AllWeapons.WeaponQueryDto;
using ArmorDescriptionDto = ArmiesService.Queries.AllArmors.ArmorQueryDto;
using System.Collections.Generic;

namespace ArmiesService.Queries
{
    public interface IQueriesFactory
    {
        IQuery<ArmyQueryDto> CreateArmyQuery();

        IQuery<IEnumerable<SquadDescriptionDto>> CreateAllSquadsQuery();

        IQuery<IEnumerable<WeaponDescriptionDto>> CreateAllWeaponsQuery();

        IQuery<IEnumerable<ArmorDescriptionDto>> CreateAllArmorsQuery();
    }
}
