using System;

namespace UsersDomain.ValueTypes.Registration
{
    public class TTL
    {
        private static readonly TimeSpan defaultTime = TimeSpan.FromHours(24);

        private readonly TimeSpan time;

        public TTL(TimeSpan time)
        {
            this.time = time;
        }

        public TimeSpan ToTimeSpan()
        {
            return time;
        }

        public static TTL CreateDefault()
        {
            return new TTL(defaultTime);
        }
    }
}
