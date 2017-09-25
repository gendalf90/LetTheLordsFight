using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Common
{
    public class MapObjectUpdateData
    {
        [Required]
        public MapPosition Destination { get; set; }
    }
}
