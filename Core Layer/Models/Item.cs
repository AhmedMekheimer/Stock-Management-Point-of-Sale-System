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
        [RegularExpression(@"^(?!\s+$).*", ErrorMessage = "Name cannot be only whitespace.")]
        public string Name { get; set; } = null!;

        [Required]
        public string Barcode { get; set; } = null!;

        [Range(0, 100, ErrorMessage = "Tax is written in percentage values from 0 to 100")]
        public int? TaxPercentage { get; set; }

        [Range(0, 100, ErrorMessage = "Discount is written in percentage values from 0 to 100")]
        public int? DiscountPercentage { get; set; }

        [Range(0,int.MaxValue)]
        public int? RestockThreshold { get; set; }

        // Many-to-Many: Item is in many Branches (Bridge Table Needed)
        public List<BranchItem> BranchItems { get; set; } = new List<BranchItem>();

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

        // An Item is included in many Operation Items
        public ICollection<OperationItem> OperationItems { get; set; } = new List<OperationItem>();

        // Navigation properties
        public Brand Brand { get; set; } = null!;
        public Color Color { get; set; } = null!;
        public ItemType ItemType { get; set; } = null!;
        public Size Size { get; set; } = null!;
        public TargetAudience TargetAudience { get; set; } = null!;
    }
}
