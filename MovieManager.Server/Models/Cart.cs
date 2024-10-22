namespace MovieManager.Server.Models
{
    public class Cart
    {
        public int Id { get; set; }
        // TODO Not compatible with EF core rn, fix later
        public List<Ticket> Tickets { get; set; }
    }
}
