namespace MovieManager.Server.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<CartItem> Tickets { get; set; } = new List<CartItem>();
        public double Total { get  {
                return Tickets.Sum(t => t.Quantity * t.Ticket.Price);
            }
        }
    }

    public class PaymentRequest
    {
        public int CartId { get; set; }
        public string CardNumber { get; set; }
        public string Exp { get; set; }
        public string CardholderName { get; set; }
        public string Cvc { get; set; }
    }
}
