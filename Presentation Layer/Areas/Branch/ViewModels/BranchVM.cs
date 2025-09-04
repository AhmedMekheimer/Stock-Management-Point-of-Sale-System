using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchVM
    {
        public int? BranchId { get; set; }

        [Required]
        [Display(Name = "Branch name")]
        public string Name { get; set; } = null!;

    }
}
