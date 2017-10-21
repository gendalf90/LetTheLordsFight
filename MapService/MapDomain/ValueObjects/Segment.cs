using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.ValueObjects
{
    public struct Segment
    {
        public Segment(int i, int j, Location leftUp, Location rightDown, SegmentType type, float speed) : this()
        {
            I = i;
            J = j;
            LeftUpLocation = leftUp;
            RightDownLocation = rightDown;
            Type = type;
            Speed = speed;
        }

        public int I { get; private set; }

        public int J { get; private set; }

        public Location LeftUpLocation { get; private set; }

        public Location RightDownLocation { get; private set; }

        public SegmentType Type { get; private set; }

        public float Speed { get; private set; }
    }
}
