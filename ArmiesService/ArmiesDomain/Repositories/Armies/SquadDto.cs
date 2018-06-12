﻿using System.Collections.Generic;

namespace ArmiesDomain.Repositories.Armies
{
    public class SquadDto
    {
        public string Type { get; set; }

        public int Quantity { get; set; }

        public List<string> Weapons { get; set; }

        public List<string> Armors { get; set; }
    }
}
