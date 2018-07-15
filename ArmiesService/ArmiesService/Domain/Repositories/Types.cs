using ArmiesDomain.Repositories.Squads;
using ArmiesDomain.Repositories.Users;
using MongoDB.Bson.Serialization;

namespace ArmiesService.Domain.Repositories
{
    static class Types
    {
        public static void Register()
        {
            BsonClassMap.RegisterClassMap<UserDto>(cm =>
            {
                cm.MapIdProperty(e => e.Login);
                cm.MapProperty(e => e.ArmyCostLimit).SetElementName("army_cost_limit");
            });
            
            BsonClassMap.RegisterClassMap<SquadDto>(cm =>
            {
                cm.MapIdProperty(e => e.Type);
                cm.MapProperty(e => e.Cost).SetElementName("cost");
                cm.MapProperty(e => e.Tags).SetElementName("tags");
            });
        }
    }
}
