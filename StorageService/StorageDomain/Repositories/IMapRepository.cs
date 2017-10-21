using StorageDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorageDomain.Repositories
{
    public interface IMapRepository
    {
        Task<Segment> GetSegmentAsync(string id);
    }
}
