using MapDomain.Common;
using MapDomain.Entities;
using MapDomain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDomain.Repositories
{
    public interface IMapObjectsRepository
    {
        IEnumerable<MapObject> GetAllMovingObjects();

        MapObject GetById(string id);

        void SaveDestination(MapObject mapObj);

        void SaveLocationAndVisible(IEnumerable<MapObject> objects);
    }
}
