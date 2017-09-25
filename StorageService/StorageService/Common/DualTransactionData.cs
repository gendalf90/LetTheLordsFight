using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Common
{
    public class DualTransactionData
    {
        [Required]
        public string FromStorageId { get; set; }

        [Required]
        public string ToStorageId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ItemCount must be greater than 0")]
        public int? ItemCount { get; set; }
    }
}
