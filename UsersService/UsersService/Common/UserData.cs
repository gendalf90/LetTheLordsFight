using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsersService.Common
{
    public class UserData : IValidatableObject
    {
        public string Login { get; set; }

        public string StorageId { get; set; }

        public string MapObjectId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(string.IsNullOrEmpty(Login))
            {
                yield return new ValidationResult($"{nameof(Login)} must be filled");
            }

            if (string.IsNullOrEmpty(StorageId))
            {
                yield return new ValidationResult($"{nameof(StorageId)} must be filled");
            }

            if (string.IsNullOrEmpty(MapObjectId))
            {
                yield return new ValidationResult($"{nameof(MapObjectId)} must be filled");
            }
        }
    }
}
