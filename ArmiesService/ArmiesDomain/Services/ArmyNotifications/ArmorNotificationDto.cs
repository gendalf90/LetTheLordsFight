using System.Collections.Generic;

namespace ArmiesDomain.Services.ArmyNotifications
{
    public class ArmorNotificationDto
    {
        public string Name { get; set; }

        public List<DefenceNotificationDto> Defence { get; set; }

        public List<string> Tags { get; set; }
    }
}
