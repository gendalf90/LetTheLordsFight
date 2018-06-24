using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmiesDomain.ValueObjects
{
    public class Offence
    {
        private Range range;
        private Tag[] tags;

        public Offence() : this(new Range(), new Tag[0])
        {
        }

        public Offence(Range range, IEnumerable<Tag> tags)
        {
            if(range == null || tags == null)
            {
                throw new ArgumentNullException($"Some argument in offence is null");
            }

            this.range = range;
            this.tags = tags.ToArray();
        }
    }
}
