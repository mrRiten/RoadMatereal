using System.ComponentModel.DataAnnotations;

namespace RoadMatereal.Models
{
    public class Supplier
    {
        [Key]
        public required int IdSupplier { get; set; }
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public ICollection<Material>? Materials { get; set; }
    }
}
