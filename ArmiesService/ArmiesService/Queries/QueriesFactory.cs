using System.Collections.Generic;
using ArmiesService.Common;
using ArmiesService.Controllers.Data;
using ArmiesService.Queries.AllArmors;
using ArmiesService.Queries.AllSquads;
using ArmiesService.Queries.AllWeapons;
using ArmiesService.Queries.Army;
using ArmiesService.Queries.User;
using MongoDB.Driver;

namespace ArmiesService.Queries
{
    class QueriesFactory : IQueriesFactory
    {
        private readonly IMongoDatabase database;
        private readonly IGetCurrentUserLoginStrategy currentUserLogin;

        public QueriesFactory(IMongoDatabase database, IGetCurrentUserLoginStrategy currentUserLogin)
        {
            this.database = database;
            this.currentUserLogin = currentUserLogin;
        }

        public IQuery<UserGetDto> CreateUserQuery()
        {
            return new UserQuery(database, currentUserLogin);
        }

        public IQuery<IEnumerable<ArmorDictionaryDto>> CreateAllArmorsQuery()
        {
            return new AllArmorsQuery(database);
        }

        public IQuery<IEnumerable<SquadDictionaryDto>> CreateAllSquadsQuery()
        {
            return new AllSquadsQuery(database);
        }

        public IQuery<IEnumerable<WeaponDictionaryDto>> CreateAllWeaponsQuery()
        {
            return new AllWeaponsQuery(database);
        }

        public IQuery<ArmyGetDto> CreateArmyQuery()
        {
            return new ArmyQuery(database, currentUserLogin);
        }
    }
}
