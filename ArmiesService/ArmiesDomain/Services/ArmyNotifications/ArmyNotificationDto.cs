using System.Collections.Generic;

namespace ArmiesDomain.Services.ArmyNotifications
{
    public class ArmyNotificationDto
    {
        public string OwnerLogin { get; set; }

        public List<SquadNotificationDto> Squads { get; set; }
    }
}
