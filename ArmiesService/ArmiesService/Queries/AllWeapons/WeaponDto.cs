using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllWeapons
{
    public class WeaponDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public OffenceDto[] Offence { get; set; }

        public string[] Tags { get; set; }
    }
}
