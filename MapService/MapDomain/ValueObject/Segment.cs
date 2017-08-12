using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.ValueObject
{
    public struct Segment
    {
        public Segment(Location leftUp, Location rightDown, SegmentType type, float speed) : this()
        {
            LeftUpLocation = leftUp;
            RightDownLocation = rightDown;
            Type = type;
            Speed = speed;
        }

        public Location LeftUpLocation { get; private set; }

        public Location RightDownLocation { get; private set; }

        public SegmentType Type { get; private set; }

        public float Speed { get; private set; }
    }
}
