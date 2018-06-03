using System.Collections.Generic;

namespace ArmiesDomain.Repositories.Armies
{
    public class SquadDto
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public List<string> Weapons { get; set; } //by name

        public List<string> Armors { get; set; } //names
    }
}
