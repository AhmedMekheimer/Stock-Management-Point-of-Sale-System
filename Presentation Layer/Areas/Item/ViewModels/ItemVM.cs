using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Stock.ViewModels
{
    public class ItemVM
    {
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public IEnumerable<SelectListItem> BrandsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ColorsList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ItemTypesList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> SizesList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TargetAudiencesList { get; set; } = new List<SelectListItem>();
    }
}
