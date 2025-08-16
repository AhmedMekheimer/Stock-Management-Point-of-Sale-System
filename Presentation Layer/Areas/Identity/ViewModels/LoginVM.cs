using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.IDentitiy.ViewModels
{
    public class LoginVM
    {

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string UserNameOrEmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
