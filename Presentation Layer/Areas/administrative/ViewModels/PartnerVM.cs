using CoreLayer.CustomValidations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static CoreLayer.Models.Partner;

namespace PresentationLayer.Areas.administrative.ViewModels
{
    public class PartnerVM
    {

        public int? Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string? Email { get; set; } 
        [Required]
        public PartnerType partnerType { get; set; } 
        [EgyptianPhoneList]
        public string? PhoneNumber { get; set; }

        public List<SelectListItem> PartnerList = new List<SelectListItem>();

    }
}
