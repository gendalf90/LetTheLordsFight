using System.Collections.Generic;
using System.Linq;

namespace ArmiesDomain.ValueObjects
{
    public class Offence
    {
        private Range range;
        private Tag[] tags;

        public Offence()
        {
            range = new Range();
            tags = new Tag[0];
        }

        public Offence(Range range, IEnumerable<Tag> tags)
        {
            this.range = range;
            this.tags = tags.ToArray();
        }
    }
}
