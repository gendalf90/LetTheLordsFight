using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Common
{
    public class MapObjectUpdateData : IValidatableObject
    {
        public MapPosition Destination { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Destination != null && (!Destination.X.HasValue || !Destination.Y.HasValue))
            {
                yield return new ValidationResult($"{nameof(Destination.X)} and {nameof(Destination.Y)} must be filled");
            }
        }
    }
}
