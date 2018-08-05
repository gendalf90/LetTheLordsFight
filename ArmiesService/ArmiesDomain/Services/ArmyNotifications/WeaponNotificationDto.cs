using System.Collections.Generic;

namespace ArmiesDomain.Services.ArmyNotifications
{
    public class WeaponNotificationDto
    {
        public string Name { get; set; }

        public List<OffenceNotificationDto> Offence { get; set; }

        public List<string> Tags { get; set; }
    }
}
