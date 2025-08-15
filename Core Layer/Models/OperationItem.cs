using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class OperationItem
    {
        public int Id { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = null!;

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}
