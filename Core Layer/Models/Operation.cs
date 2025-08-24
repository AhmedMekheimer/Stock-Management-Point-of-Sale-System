using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public abstract class Operation
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double TotalItemsPrice { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double TotalTaxes { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double GrandTotal { get; set; }
        [Required]
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public ICollection<OperationItem> OperationItems { get; set; } = new List<OperationItem>();
    }
}
