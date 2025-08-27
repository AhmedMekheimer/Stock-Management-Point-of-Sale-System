using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class DiscountOperation
    {
        public int DiscountId { get; set; }
        public Discount Discount { get; set; } = null!;
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = null!;
    }
}
