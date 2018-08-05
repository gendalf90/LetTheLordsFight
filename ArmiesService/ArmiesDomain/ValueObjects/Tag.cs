using ArmiesDomain.Services.ArmyNotifications;
using System;

namespace ArmiesDomain.ValueObjects
{
    public class Tag
    {
        private readonly string value;

        public Tag(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Tag value is null or empty");
            }

            this.value = value;
        }

        public void FillDefenceData(DefenceNotificationDto data)
        {
            data.Tags.Add(value);
        }

        public void FillOffenceData(OffenceNotificationDto data)
        {
            data.Tags.Add(value);
        }

        public void FillWeaponData(WeaponNotificationDto data)
        {
            data.Tags.Add(value);
        }

        public void FillArmorData(ArmorNotificationDto data)
        {
            data.Tags.Add(value);
        }

        public void FillSquadData(SquadNotificationDto data)
        {
            data.Tags.Add(value);
        }
    }
}
