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
        public enum OperationType
        {
            ReceiveOrder = 1,
            SalesInvoice = 2,
            BranchesTransfer = 3
        }
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public OperationType operationType { get; set; }
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
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }=new ApplicationUser();
        public ICollection<OperationItem> OperationItems { get; set; } = new List<OperationItem>();
    }
}
