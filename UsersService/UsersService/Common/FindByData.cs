using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Common
{
    public class FindByData : IValidatableObject
    {
        public string StorageId { get; set; }

        public string MapObjectId { get; set; }

        public string Login { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fields = new object[]
            {
                StorageId,
                MapObjectId,
                Login
            };

            var presentCount = fields.Count(field => field != null);

            if(presentCount != 1)
            {
                yield return new ValidationResult($"Parameter must be single");
            }
        }
    }
}
