using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDomain.ValueObjects
{
    public class Segment : IEquatable<Segment>
    {
        private int i;
        private int j;

        public Segment(int i, int j)
        {
            this.i = i;
            this.j = j;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Segment);
        }

        public override int GetHashCode()
        {
            return i.GetHashCode() ^ j.GetHashCode();
        }

        public bool Equals(Segment other)
        {
            return other?.i == i && other?.j == j;
        }
    }
}
