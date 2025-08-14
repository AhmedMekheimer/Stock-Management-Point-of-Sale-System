using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class Invoice:Operation
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int RetailCustomerId { get; set; }  // FK to Partner (CorporateCustomer)
        public int? VoucherId { get; set; }

        // Navigation properties
        public Branch Branch { get; set; }
        public Partner RetailCustomer { get; set; }
    }
}
