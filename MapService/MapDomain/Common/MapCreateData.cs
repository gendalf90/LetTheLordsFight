using MapDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.Common
{
    public class MapCreateData
    {
        public SegmentType[,] Types { get; set; }

        public float SegmentSize { get; set; }

        public Dictionary<SegmentType, float> SegmentsSpeed { get; set; }
    }
}
