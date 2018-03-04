using System;
using System.Collections.Generic;
using System.Text;

namespace UsersDomain.ValueTypes.Registration
{
    public class TTL
    {
        public TTL()
        {
            //set defaults
        }

        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan();
        }
    }
}
