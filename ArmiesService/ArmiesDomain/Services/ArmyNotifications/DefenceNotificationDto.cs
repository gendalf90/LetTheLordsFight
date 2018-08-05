using System.Collections.Generic;

namespace ArmiesDomain.Services.ArmyNotifications
{
    public class DefenceNotificationDto
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public List<string> Tags { get; set; }
    }
}
