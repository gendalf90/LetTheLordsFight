using System.Collections.Generic;
using ArmiesService.Common;
using ArmiesService.Queries.AllArmors;
using ArmiesService.Queries.AllWeapons;
using ArmiesService.Queries.Army;
using MongoDB.Driver;
using AllArmorsQuery = ArmiesService.Queries.AllArmors.AllArmorsQuery;
using AllSquadsQuery = ArmiesService.Queries.AllSquads.AllSquadsQuery;
using AllWeaponsQuery = ArmiesService.Queries.AllWeapons.AllWeaponsQuery;
using ArmyQuery = ArmiesService.Queries.Army.ArmyQuery;

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

        public IQuery<IEnumerable<ArmorQueryDto>> CreateAllArmorsQuery()
        {
            return new AllArmorsQuery(database);
        }

        public IQuery<IEnumerable<AllSquads.SquadQueryDto>> CreateAllSquadsQuery()
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
