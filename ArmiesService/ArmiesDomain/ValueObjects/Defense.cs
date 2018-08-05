using ArmiesDomain.Services.ArmyNotifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmiesDomain.ValueObjects
{
    public class Defense
    {
        private Range range;
        private List<Tag> tags;

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
            this.tags = tags.ToList();
        }

        public void FillArmorData(ArmorNotificationDto data)
        {
            var defenceDto = new DefenceNotificationDto
            {
                Tags = new List<string>()
            };

            range.FillDefenceData(defenceDto);
            tags.ForEach(tag => tag.FillDefenceData(defenceDto));
            data.Defence.Add(defenceDto);
        }
    }
}
