using CoreLayer.Models.ItemVarients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1000000000000, 9999999999999, ErrorMessage = "Bar Code Number must be a 13-digit Number")]
        public Int64 BarcodeNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, 100, ErrorMessage = "Tax is written in percentage values from 0 to 100")]
        public int? TaxPercentage { get; set; }

        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int? DiscountPercentage { get; set; }
        [Range(0,int.MaxValue)]
        public int? RestockThreshold { get; set; }
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();

        //Item Varients FKs
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int ColorId { get; set; }
        [Required]
        public int ItemTypeId { get; set; }
        [Required]
        public int SizeId { get; set; }
        [Required]
        public int TargetAudienceId { get; set; }

        // Navigation properties
        public Brand Brand { get; set; }
        public Color Color { get; set; }
        public ItemType ItemType { get; set; }
        public Size Size { get; set; }
        public TargetAudience TargetAudience { get; set; }
        public ICollection<OperationItem> OperationItems { get; set; } = new List<OperationItem>();
        public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();
    }
}
