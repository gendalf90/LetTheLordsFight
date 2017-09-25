using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.ValueObjects
{
    public class Coordinate : IEquatable<Coordinate>
    {
        private float x;
        private float y;

        public Coordinate(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public bool Equals(Coordinate other)
        {
            return other?.x == x && other?.y == y;
        }
    }
}
