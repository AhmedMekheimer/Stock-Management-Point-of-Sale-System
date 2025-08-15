using CoreLayer.CustomValidations;
using CoreLayer.Models.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class Partner
    {
        public enum PartnerType
        {
            Supplier = 1,
            CorporateCustomer = 2,
            RetailCustomer = 3
        }
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;
        [Required]
        public PartnerType partnerType { get; set; } = 0;
        [Required]
        [EgyptianPhoneList]
        public List<string> PhoneNumbers { get; set; } = new List<string>();

        public ICollection<SalesInvoice>? CorporateSales { get; set; } = new List<SalesInvoice>();
        public ICollection<ReceiveOrder>? SupplyOrders { get; set; } = new List<ReceiveOrder>();
        public ICollection<Invoice>? Invoices { get; set; } = new List<Invoice>();
    }
}
