using System.Collections.Generic;
using ArmiesService.Common;
using ArmiesService.Queries.AllArmors;
using ArmiesService.Queries.AllSquads;
using ArmiesService.Queries.AllWeapons;
using ArmiesService.Queries.Army;
using ArmiesService.Queries.User;
using MongoDB.Driver;
using SquadDictionaryQueryDto = ArmiesService.Queries.AllSquads.SquadQueryDto;

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

        public IQuery<UserQueryDto> CreateUserQuery()
        {
            return new UserQuery(database, currentUserLogin);
        }

        public IQuery<IEnumerable<ArmorQueryDto>> CreateAllArmorsQuery()
        {
            return new AllArmorsQuery(database);
        }

        public IQuery<IEnumerable<SquadDictionaryQueryDto>> CreateAllSquadsQuery()
        {
            return new AllSquadsQuery(database);
        }

        public IQuery<IEnumerable<WeaponQueryDto>> CreateAllWeaponsQuery()
        {
            return new AllWeaponsQuery(database);
        }

        public IQuery<ArmyQueryDto> CreateArmyQuery()
        {
            return new ArmyQuery(database, currentUserLogin);
        }
    }
}
