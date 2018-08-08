using ArmiesService.Controllers.Data;
using System.Collections.Generic;

namespace ArmiesService.Queries
{
    public interface IQueriesFactory
    {
        IQuery<ArmyGetDto> CreateArmyQuery();

        IQuery<UserGetDto> CreateUserQuery();

        IQuery<IEnumerable<SquadDictionaryDto>> CreateAllSquadsQuery();

        IQuery<IEnumerable<WeaponDictionaryDto>> CreateAllWeaponsQuery();

        IQuery<IEnumerable<ArmorDictionaryDto>> CreateAllArmorsQuery();
    }
}
