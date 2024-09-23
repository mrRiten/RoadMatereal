using System.ComponentModel.DataAnnotations;

namespace RoadMatereal.Models
{
    public class Order
    {
        [Key]
        public required int IdOrder { get; set; }
        public required DateTime Date { get; set; }

        public required int ClientID { get; set; }
        public ApplicationUser? Client { get; set; }

        public required int StatusID { get; set; }
        public Status? Status { get; set; }

        public int Count { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }

}
