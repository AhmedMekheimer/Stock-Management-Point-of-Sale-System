using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stock.ViewModels
{
    public class ItemVM
    {
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Barcode { get; set; } = null!;
        [Required]
        public int SizeId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int ColorId { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        [Required]
        public int TargetAudienceId { get; set; }
        public IEnumerable<SelectListItem> BrandsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ColorsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ItemTypesList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SizesList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TargetAudiencesList { get; set; } = new List<SelectListItem>();
    }
}
