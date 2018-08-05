using System.Collections.Generic;

namespace ArmiesDomain.Services.ArmyNotifications
{
    public class SquadNotificationDto
    {
        public string Type { get; set; }

        public int Quantity { get; set; }

        public List<WeaponNotificationDto> Weapons { get; set; }

        public List<ArmorNotificationDto> Armors { get; set; }

        public List<string> Tags { get; set; }
    }
}
