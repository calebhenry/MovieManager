using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManager.Server.Models
{
    /// <summary>
    /// Represents a shopping cart containing a list of tickets and the total cost.
    /// </summary>
    public class Cart
    {
        public int Id { get; set; }

        [InverseProperty("Cart")]
        public List<CartItem> Tickets { get; set; } = new List<CartItem>();
        public double Total { get  {
                return Tickets.Sum(t => t.Quantity * t.Ticket.Price);
            }
        }
    }
    /// <summary>
    /// Represents a request to process a payment for a specific cart.
    /// </summary>
    public class PaymentRequest
    {
        public int CartId { get; set; }
        public string CardNumber { get; set; }
        public string Exp { get; set; }
        public string CardholderName { get; set; }
        public string Cvc { get; set; }
    }
}
