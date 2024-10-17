namespace MovieManager.Server.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
