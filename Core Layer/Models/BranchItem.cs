using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class BranchItem
    {
        public int BranchId { get; set; }
        public int ItemId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }  // <-- Your custom column

        // Navigation properties
        public Branch Branch { get; set; }=new Branch();
        public Item Item { get; set; } = new Item();
    }
}
