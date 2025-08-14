using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.ItemVarients
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
