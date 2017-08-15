using Cassandra;
using Cassandra.Mapping;
using MapDomain.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Extensions
{
    static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCassandra(this IServiceCollection serviceCollection, CassandraConfiguration configuration)
        {
            var cluster = Cluster.Builder()
                                 .AddContactPoints(configuration.Nodes)
                                 .WithCredentials(configuration.Login, configuration.Password)
                                 .Build();
            var session = cluster.Connect("map");
            return serviceCollection.AddSingleton(session);
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            MappingConfiguration.Global.Define(new Map<MapObjectRepositoryData>().TableName("objects").PartitionKey(data => data.Id));

        }
    }
}
