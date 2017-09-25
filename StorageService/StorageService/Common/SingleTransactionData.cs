using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageService.Common
{
    public class SingleTransactionData
    {
        [Required]
        public string StorageId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ItemCount must be greater than 0")]
        public int? ItemCount { get; set; }
    }
}
