using ArmiesDomain.Services.ArmyNotifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmiesDomain.ValueObjects
{
    public class Offence
    {
        private Range range;
        private List<Tag> tags;

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
            this.tags = tags.ToList();
        }

        public void FillWeaponData(WeaponNotificationDto data)
        {
            var offenceDto = new OffenceNotificationDto
            {
                Tags = new List<string>()
            };

            range.FillOffenceData(offenceDto);
            offenceDto.Tags = tags.Select(tag => tag.ToString()).ToList();
            data.Offence.Add(offenceDto);
        }
    }
}
