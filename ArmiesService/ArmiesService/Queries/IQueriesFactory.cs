using ArmiesService.Queries.Army;
using System.Collections.Generic;
using SquadDictionaryQueryDto = ArmiesService.Queries.AllSquads.SquadQueryDto;
using ArmiesService.Queries.AllWeapons;
using ArmiesService.Queries.AllArmors;
using ArmiesService.Queries.User;

namespace ArmiesService.Queries
{
    public interface IQueriesFactory
    {
        IQuery<ArmyQueryDto> CreateArmyQuery();

        IQuery<UserQueryDto> CreateUserQuery();

        IQuery<IEnumerable<SquadDictionaryQueryDto>> CreateAllSquadsQuery();

        IQuery<IEnumerable<WeaponQueryDto>> CreateAllWeaponsQuery();

        IQuery<IEnumerable<ArmorQueryDto>> CreateAllArmorsQuery();
    }
}
