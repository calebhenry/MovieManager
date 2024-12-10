using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    /// <summary>
    /// Represents the item(s) (ticket) in one cart. 
    /// </summary>
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public int Quantity { get; set; }

        // EF core link
        [JsonIgnore]
        public Cart Cart { get; set; } = null!;
        public Ticket Ticket { get; set; } = null!;
    }
}
