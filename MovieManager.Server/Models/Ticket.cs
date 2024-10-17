namespace MovieManager.Server.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public Showtime Showtime { get; set; }
    }
}
