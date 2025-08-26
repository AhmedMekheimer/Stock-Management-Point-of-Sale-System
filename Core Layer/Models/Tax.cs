using CoreLayer.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    [EitherOr("Rate", "RawValue")]
    public class Tax
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;
        [Range(0, 100)]
        public int? Rate { get; set; } = 0;
        public int? RawValue { get; set; } = 0;
        public ICollection<TaxReceiveOrder> TaxReceiveOrders { get; set; } = new List<TaxReceiveOrder>();
    }
}
