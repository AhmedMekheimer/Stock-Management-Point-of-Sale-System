using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models.ItemVarients
{
    public class ItemType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        // Self-referencing FK (nullable for root categories)
        public int? ItemTypeId { get; set; }
        public ItemType? Parent { get; set; }                 //Navigation to Parent
        public List<ItemType> Children { get; set; } = new List<ItemType>(); // Navigation to children
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
