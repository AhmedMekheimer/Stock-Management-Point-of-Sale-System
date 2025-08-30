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
        public Branch Branch { get; set; } = null!;

        public int? RetailCustomerId { get; set; }  // FK to Partner
        public Partner RetailCustomer { get; set; } = null!;
        public ICollection<DiscountSalesInvoice> DiscountSalesInvoices { get; set; } = new List<DiscountSalesInvoice>();
    }
}
