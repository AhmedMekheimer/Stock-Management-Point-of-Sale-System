using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"\S+", ErrorMessage = "Code cannot be empty or whitespace.")]
        public string Code { get; set; } = null!;
        [Required]
        public double DiscountValue { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public int? MaximumUses { get; set; }
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}
