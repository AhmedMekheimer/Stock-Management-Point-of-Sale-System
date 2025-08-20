using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchVM
    {

        [Required]
        [Display(Name = "Branch name")]
        public string BranchName { get; set; } = null!;
        [Required]
        [Display(Name = "Branch Manager")]
        public string BranchManagerId{ get; set; } = null!;

        public IEnumerable<SelectListItem> UsersList { get; set; } = new List<SelectListItem>();
    }
}
