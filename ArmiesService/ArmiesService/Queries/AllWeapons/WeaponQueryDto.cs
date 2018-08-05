using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesService.Queries.AllWeapons
{
    public class WeaponQueryDto
    {
        public string Name { get; set; }

        public int Cost { get; set; }

        public OffenceQueryDto[] Offence { get; set; }

        public string[] Tags { get; set; }
    }
}
