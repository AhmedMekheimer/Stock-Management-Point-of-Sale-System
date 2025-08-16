﻿using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.IDentitiy.ViewModels
{
    public class ProfileEditVM
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string UserName { get; set; } = null!;

  

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        public bool ConfirmEmail { get; set; }

        [MinLength(10)]
        [MaxLength(100)]
    

        public List<string> Roles { get; set; } = new();

    }
}
