using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Tax
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;
        [Range(1,100)]
        public int? Rate { get; set; }
        public int? RawValue { get; set; }
        public ICollection<TaxReceiveOrder> TaxReceiveOrders { get; set; } = new List<TaxReceiveOrder>();
    }
}
