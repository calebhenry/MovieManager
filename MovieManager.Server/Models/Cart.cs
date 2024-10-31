namespace MovieManager.Server.Models
{
    public class Cart
    {
        public int Id { get; set; }
        // TODO Not compatible with EF core rn, fix later
        public List<CartItem> Tickets { get; set; }
        public double Total { get  {
                return Tickets.Sum(t => t.Quantity * t.Ticket.Price);
            }
        }
    }
}
