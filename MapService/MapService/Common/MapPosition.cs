using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Common
{
    public class MapPosition
    {
        [Required]
        public float? X { get; set; }

        [Required]
        public float? Y { get; set; }
    }
}
