namespace MovieManager.Server.Models
{
    public class Cart
    {
        public int Id { get; set; }
        // TODO Not compatible with EF core rn, fix later
        // maps Tickets to the quanity
        public Dictionary<Ticket, int> Tickets { get; set; }
        public double Total { get  {
                return Tickets.Sum(t => t.Key.Price);
            }
        }
    }
}
