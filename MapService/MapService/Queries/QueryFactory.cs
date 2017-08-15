using Cassandra;
using MapDomain.Factories;
using MapDomain.Repositories;
using MapDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IUserValidationService userValidationService;
        private readonly IMapFactory mapFactory;
        private readonly ISession session;

        public QueryFactory(ISession session, IMapFactory mapFactory, IUserValidationService userValidationService)
        {
            this.userValidationService = userValidationService;
            this.mapFactory = mapFactory;
            this.session = session;
        }

        public IQuery CreateMapQuery()
        {
            return new MapQuery(mapFactory);
        }

        public IQuery CreateObjectQuery(string id)
        {
            return new ObjectQuery(session, userValidationService, id);
        }

        public IQuery CreateSegmentQuery(int i, int j)
        {
            return new SegmentQuery(session, mapFactory, i, j);
        }

        public IQuery CreateSegmentQuery(float x, float y)
        {
            return new SegmentQuery(session, mapFactory, x, y);
        }
    }
}
