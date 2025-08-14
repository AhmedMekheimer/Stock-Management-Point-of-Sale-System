using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        // One-to-Many: Branch has many cashiers
        public List<ApplicationUser> Cashiers { get; set; } = new List<ApplicationUser>();

        // One-to-One: Branch has one manager
        [Required]
        public int BranchManagerId { get; set; }  // Foreign key for manager
        public ApplicationUser BranchManager { get; set; }
        // Nav properties
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<Invoice>? Invoices { get; set; } = new List<Invoice>();
        public ICollection<SalesInvoice>? SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<ReceiveOrder>? ReceiveOrders { get; set; } = new List<ReceiveOrder>();
        public List<Transfer>? OutgoingTransfers { get; set; }  // Transfers FROM this branch
        public List<Transfer>? IncomingTransfers { get; set; }  // Transfers TO this branch
    }
}
