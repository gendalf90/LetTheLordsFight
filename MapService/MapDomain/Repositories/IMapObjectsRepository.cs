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
        Task<IEnumerable<MapObject>> GetAllMovingObjectsAsync();

        Task<MapObject> GetByIdAsync(string id);

        Task SaveDestinationAsync(MapObject mapObj);

        Task SaveLocationAndVisibleAsync(IEnumerable<MapObject> objects);
    }
}
