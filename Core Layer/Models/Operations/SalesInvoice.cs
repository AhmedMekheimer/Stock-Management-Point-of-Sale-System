using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class SalesInvoice : Operation
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int CorporateCustomerId { get; set; } // FK to Partner (CorporateCustomer)

        // Navigation properties
        public Branch Branch { get; set; } = new Branch();
        public Partner CorporateCustomer { get; set; } = new Partner();
    }
}
