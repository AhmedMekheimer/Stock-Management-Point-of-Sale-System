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
        public int Id { get; set; }
        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; }
    }
}
