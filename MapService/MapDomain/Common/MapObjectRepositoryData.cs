using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Common
{
    public class MapObjectRepositoryData
    {
        public string Id { get; set; }

        public float LocationX { get; set; }

        public float LocationY { get; set; }

        public float DestinationX { get; set; }

        public float DestinationY { get; set; }

        public bool IsVisible { get; set; }
    }
}
