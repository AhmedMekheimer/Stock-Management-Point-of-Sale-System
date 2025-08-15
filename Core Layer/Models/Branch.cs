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
        public ICollection<ApplicationUser> Cashiers { get; set; } = new List<ApplicationUser>();

        // One-to-One: Branch has one manager
        [Required]
        public string BranchManagerId { get; set; } = null!;  // Foreign key for manager
        public ApplicationUser BranchManager { get; set; } = new ApplicationUser();

        // Many-to-Many: Branch has many Items (Bridge Table Needed)
        public ICollection<Item> Items { get; set; } = new List<Item>();

        // One-to-Many: Branch has many Operations
        public ICollection<Invoice>? Invoices { get; set; } = new List<Invoice>();
        public ICollection<SalesInvoice>? SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<ReceiveOrder>? ReceiveOrders { get; set; } = new List<ReceiveOrder>();
        public ICollection<Transfer>? OutgoingTransfers { get; set; } = new List<Transfer>();  // Transfers FROM this branch
        public ICollection<Transfer>? IncomingTransfers { get; set; } = new List<Transfer>();  // Transfers TO this branch
    }
}
