using MapDomain.Common;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapService.Repositories
{
    public class MapObjectModel : IMapObjectRepositoryData
    {
        public string Id { get; set; }

        public float LocationX { get; set; }

        public float LocationY { get; set; }

        public float DestinationX { get; set; }

        public float DestinationY { get; set; }

        public bool IsVisible { get; set; }

        public bool IsMoving { get; set; }
    }
}
