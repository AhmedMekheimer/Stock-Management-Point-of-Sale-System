using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class ReceiveOrder : Operation
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public int SupplierId { get; set; } // FK to Partner (Supplier)

        // Navigation properties
        public Branch Branch { get; set; }
        public Partner Supplier { get; set; }
    }
}
