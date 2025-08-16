using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.IDentitiy.ViewModels
{
    public class EmailConfirmationVM
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string UserNameOrEmail { get; set; } = null!;

    }
}
