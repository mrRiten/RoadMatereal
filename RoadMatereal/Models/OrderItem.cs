using System.ComponentModel.DataAnnotations;

namespace RoadMatereal.Models
{
    public class OrderItem
    {
        [Key]
        public required int IdOrderItem { get; set; }

        public required int OrderID { get; set; }
        public Order? Order { get; set; }

        public required int MaterialID { get; set; }
        public Material? Material { get; set; }

        public int Count { get; set; }
        public required decimal Price { get; set; }
    }
}
