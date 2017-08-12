using Cassandra;
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
        private readonly IMapRepository mapRepository;
        private readonly ISession session;

        public QueryFactory(ISession session, IMapRepository mapRepository, IUserValidationService userValidationService)
        {
            this.userValidationService = userValidationService;
            this.mapRepository = mapRepository;
            this.session = session;
        }

        public IQuery CreateMapQuery()
        {
            return new MapQuery(mapRepository);
        }

        public IQuery CreateObjectQuery(string id)
        {
            return new ObjectQuery(session, userValidationService, id);
        }

        public IQuery CreateSegmentQuery(int i, int j)
        {
            return new SegmentQuery(session, mapRepository, i, j);
        }

        public IQuery CreateSegmentQuery(float x, float y)
        {
            return new SegmentQuery(session, mapRepository, x, y);
        }
    }
}
