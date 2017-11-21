using MapDomain.Factories;
using MapDomain.Repositories;
using MapDomain.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    class QueryFactory : IQueryFactory
    {
        private readonly IUserValidationService userValidationService;
        private readonly IMapFactory mapFactory;
        private readonly IMongoDatabase mapDatabase;

        public QueryFactory(IMongoDatabase mapDatabase, IMapFactory mapFactory, IUserValidationService userValidationService)
        {
            this.userValidationService = userValidationService;
            this.mapFactory = mapFactory;
            this.mapDatabase = mapDatabase;
        }

        public IQuery CreateMapQuery()
        {
            return new MapQuery(mapFactory);
        }

        public IQuery CreateObjectQuery(string id)
        {
            return new ObjectQuery(mapDatabase, mapFactory, userValidationService, id);
        }

        public IQuery CreateSegmentQuery(int i, int j)
        {
            return new SegmentQuery(mapDatabase, mapFactory, i, j);
        }

        public IQuery CreateSegmentQuery(float x, float y)
        {
            return new SegmentQuery(mapDatabase, mapFactory, x, y);
        }

        public IQuery CreateSquare5x5Query(int i, int j)
        {
            return new Square5x5Query(mapDatabase, mapFactory, i, j);
        }
    }
}
