using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class TransactionItem
    {
        public int Id { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double TotalPrice { get; set; }

        [Required]
        public int TransactionId { get; set; }
        [Required]
        public int ItemId { get; set; }
    }
}
