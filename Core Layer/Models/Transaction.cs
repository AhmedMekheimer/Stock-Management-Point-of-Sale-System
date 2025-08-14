using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Transaction
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
        [Required]
        public int OperationId { get; set; }
        [Required]
        public OperationType operationType { get; set; } = 0;
    }
}
