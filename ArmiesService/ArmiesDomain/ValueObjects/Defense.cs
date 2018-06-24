using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmiesDomain.ValueObjects
{
    public class Defense
    {
        private Range range;
        private Tag[] tags;

        public Defense() : this(new Range(), new Tag[0])
        {
        }

        public Defense(Range range, IEnumerable<Tag> tags)
        {
            if (range == null || tags == null)
            {
                throw new ArgumentNullException($"Some argument in defence is null");
            }

            this.range = range;
            this.tags = tags.ToArray();
        }
    }
}
