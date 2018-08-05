using ArmiesDomain.Services.ArmyNotifications;
using System;

namespace ArmiesDomain.ValueObjects
{
    public class Range
    {
        private readonly int min;
        private readonly int max;

        public Range() : this(0, 0)
        {
        }

        public Range(int min, int max)
        {
            if (max < 0 || min < 0)
            {
                throw new ArgumentOutOfRangeException($"Min = {min} or Max = {max} less than 0");
            }

            if (max < min)
            {
                throw new ArgumentException($"Max = {max} should be greater than Min = {min}");
            }

            this.min = min;
            this.max = max;
        }

        public void FillOffenceData(OffenceNotificationDto data)
        {
            data.Max = max;
            data.Min = min;
        }

        public void FillDefenceData(DefenceNotificationDto data)
        {
            data.Max = max;
            data.Min = min;
        }
    }
}
