using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public override string UserName { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public override string Email { get; set; } = null!;
        // Cashier relationship (many-to-one)
        public int? BranchId { get; set; }  // Nullable for cashiers
        public Branch Branch { get; set; }  // Navigation to branch for cashiers

        // Manager relationship (one-to-one)
        public Branch ManagedBranch { get; set; }  // Navigation to managed branch

        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}
