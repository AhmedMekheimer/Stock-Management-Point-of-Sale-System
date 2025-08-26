using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Discount
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;
        [Range(1, 100)]
        public int? Rate { get; set; }
        public int? RawValue { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public int? MaximumUses { get; set; }
        public ICollection<DiscountOperation> DiscountOperations { get; set; } = new List<DiscountOperation>();
    }
}
