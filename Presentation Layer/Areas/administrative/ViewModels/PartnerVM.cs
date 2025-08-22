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
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;
        [Required]
        public PartnerType partnerType { get; set; } 
        [Required]
        [EgyptianPhoneList]
        public string PhoneNumber { get; set; } = null!;

        public List<SelectListItem> PartnerList = new List<SelectListItem>();

    }
}
