using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Common
{
    public class DualTransactionData : IValidatableObject
    {
        public string FromStorageId { get; set; }

        public string ToStorageId { get; set; }

        public string ItemName { get; set; }

        public int ItemCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ItemName))
            {
                yield return new ValidationResult($"{nameof(ItemName)} is empty");
            }

            if (ItemCount <= 0)
            {
                yield return new ValidationResult($"{nameof(ItemCount)} must be greater than 0");
            }

            if (string.IsNullOrEmpty(FromStorageId))
            {
                yield return new ValidationResult($"{nameof(FromStorageId)} is empty");
            }

            if (string.IsNullOrEmpty(ToStorageId))
            {
                yield return new ValidationResult($"{nameof(ToStorageId)} is empty");
            }
        }
    }
}
