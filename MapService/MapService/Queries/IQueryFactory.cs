using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public interface IQueryFactory
    {
        IQuery CreateMapQuery();

        IQuery CreateSegmentQuery(int i, int j);

        IQuery CreateSegmentQuery(float x, float y);

        IQuery CreateObjectQuery(string id);

        IQuery CreateSquare5x5Query(int i, int j);
    }
}
