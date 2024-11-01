using System.ComponentModel.DataAnnotations;

namespace RoadMatereal.Models
{
    public class Supplier
    {
        [Key]
        public required int IdSupplier { get; set; }
        public required string Name { get; set; }

        // Optional fields for supplier contact information
        public string? Phone { get; set; }
        public string? Email { get; set; }

        // Navigation property for materials supplied by this supplier
        public ICollection<Material>? Materials { get; set; }
    }
}