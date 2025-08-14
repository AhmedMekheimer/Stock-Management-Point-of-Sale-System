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
        public string Code { get; set; } = null!;
        [Required]
        public double DiscountValue { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
