using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Administrative.ViewModels
{
    public class CreateUser
    {
        [Required]
        [MinLength(5)]

        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(5)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(5)]
        public string Password { get; set; } = null!;


        public string? UserId { get; set; }


        public int? BranchId { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<SelectListItem> BranchList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> RolesList { get; set; } = new List<SelectListItem>();
    }
}
