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

        // Price per unit at the moment of sale
        [Required]
        [Range(0.0, double.MaxValue)]
        public double SellingPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int? DiscountRate { get; set; }

        // Quantity * SellingPrice * (1 - DiscountRate/100)
        [Required]
        [Range(0.0, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Required]
        // Item's Name at the moment of sale
        public string ItemNameSnapshot { get; set; } = null!;

        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = null!;

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}
