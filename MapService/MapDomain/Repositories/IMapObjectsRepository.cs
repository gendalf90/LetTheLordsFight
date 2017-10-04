using MapDomain.Common;
using MapDomain.Entities;
using MapDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDomain.Repositories
{
    public interface IMapObjectsRepository
    {
        Task AddAsync(MapObject obj);

        Task<IEnumerable<MapObject>> GetAllMovingObjectsAsync();

        Task<MapObject> GetByIdAsync(string id);

        Task UpdateDestinationAsync(MapObject mapObj);

        Task UpdateLocationAndVisibleAsync(IEnumerable<MapObject> objects);
    }
}
