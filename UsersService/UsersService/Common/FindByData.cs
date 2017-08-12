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

        public bool IsStorageIdFilled { get => !string.IsNullOrEmpty(StorageId); }

        public string MapObjectId { get; set; }

        public bool IsMapObjectIdFilled { get => !string.IsNullOrEmpty(MapObjectId); }

        public string Login { get; set; }

        public bool IsLoginFilled { get => !string.IsNullOrEmpty(Login); }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var filledResults = new[]
            {
                IsStorageIdFilled,
                IsMapObjectIdFilled,
                IsLoginFilled
            };

            var filledCount = filledResults.Count(result => result);

            if(filledCount != 1)
            {
                yield return new ValidationResult($"Finding parameter must be single");
            }
        }
    }
}
