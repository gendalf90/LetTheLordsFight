﻿using System.Collections.Generic;
using System.Linq;

namespace ArmiesDomain.ValueObjects
{
    public class Defense
    {
        private Range range;
        private Tag[] tags;

        public Defense(Range range, IEnumerable<Tag> tags)
        {
            this.range = range;
            this.tags = tags.ToArray();
        }
    }
}
