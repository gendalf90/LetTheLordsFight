using MapDomain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Common
{
    public class MapObjectRepositoryData
    {
        public string Id { get; set; }

        public Location Location { get; set; }

        public Location Destination { get; set; }

        public bool IsVisible { get; set; }
    }
}
