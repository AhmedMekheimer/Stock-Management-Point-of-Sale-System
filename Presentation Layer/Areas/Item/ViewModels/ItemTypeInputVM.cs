using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Areas.Item.ViewModels
{
    // Slim node used by the tree (we load children on demand)
    public record ItemTypeNodeVM(int Id, string Name, int? ParentId, bool HasChildren);

    // For create/edit forms
    public class ItemTypeInputVM
    {
        public int? ParentId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? ParentName { get; set; } // display only
    }
}
