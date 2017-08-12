using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Common
{
    public class MapObjectCreateData : IValidatableObject
    {
        public MapPosition Position { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Position == null || !Position.X.HasValue || !Position.Y.HasValue)
            {
                yield return new ValidationResult($"{nameof(Position)} must be filled");
            }
        }
    }
}
