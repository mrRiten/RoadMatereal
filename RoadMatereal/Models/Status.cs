using System.ComponentModel.DataAnnotations;

namespace RoadMatereal.Models
{
    public class Status
    {
        [Key]
        public required int IdStatus { get; set; }
        public required string Name { get; set; }

        // Navigation property for orders associated with this status
        public ICollection<Order>? Orders { get; set; }
    }
}