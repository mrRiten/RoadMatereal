using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadMatereal.Models
{
    public class Material
    {
        [Key]
        public required int IdMaterial { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Unit { get; set; }
        public required decimal Price { get; set; }
        [DefaultValue(0)]
        public int Count { get; set; }

        public required int SupplierID { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
