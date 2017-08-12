using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.ValueObject
{
    public struct Location : IEquatable<Location>
    {
        public Location(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public float X { get; private set; }

        public float Y { get; private set; }

        public bool Equals(Location other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if(obj is Location)
            {
                return Equals((Location)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Location one, Location two)
        {
            return one.Equals(two);
        }
        
        public static bool operator !=(Location one, Location two)
        {
            return !(one == two);
        }
    }
}
