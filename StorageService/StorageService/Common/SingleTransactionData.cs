using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageService.Common
{
    public class SingleTransactionData : IValidatableObject
    {
        public string StorageId { get; set; }

        public string ItemName { get; set; }

        public int ItemCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(string.IsNullOrEmpty(StorageId))
            {
                yield return new ValidationResult($"{nameof(StorageId)} is empty");
            }

            if (string.IsNullOrWhiteSpace(ItemName))
            {
                yield return new ValidationResult($"{nameof(ItemName)} is empty");
            }

            if(ItemCount <= 0)
            {
                yield return new ValidationResult($"{nameof(ItemCount)} must be greater than 0");
            }
        }
    }
}
