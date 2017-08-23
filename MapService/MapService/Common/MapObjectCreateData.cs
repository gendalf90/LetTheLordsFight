using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Common
{
    public class MapObjectCreateData : IValidatableObject
    {
        public MapPosition Location { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Location == null || !Location.X.HasValue || !Location.Y.HasValue)
            {
                yield return new ValidationResult($"{nameof(Location)} must be filled");
            }
        }
    }
}
