using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }

        // EF core link
        [JsonIgnore]
        public Cart Cart { get; set; } = null!;
        public Ticket Ticket { get; set; } = null!;
    }

    public class TicketUpdate
    {
        public int CartId { get; set; }
        public int MovieId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
