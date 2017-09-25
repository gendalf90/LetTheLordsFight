using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Common
{
    public class MapObjectCreateData
    {
        [Required]
        public MapPosition Location { get; set; }
    }
}
