using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.Operations
{
    public class Transfer:Operation
    {
        [Required]
        public int FromBranchId { get; set; }   // Manually made FK
        [Required]
        public int ToBranchId { get; set; }     // Manually made FK

        // Navigation properties
        public Branch FromBranch { get; set; }  // Source branch
        public Branch ToBranch { get; set; }    // Destination branch
    }
}
