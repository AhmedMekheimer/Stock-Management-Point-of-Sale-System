﻿using System;
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
        public int Quantity { get; set; }

        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int? DiscountRate { get; set; }
        public double BuyingPriceAvg { get; set; }
        public double LastBuyingPrice { get; set; }
        public double? SellingPrice { get; set; }
        [Range(0, int.MaxValue)]
        public int? RestockThreshold { get; set; }

        // Navigation properties
        public Branch Branch { get; set; }= null!;
        public Item Item { get; set; } = null!;
    }
}
