namespace MovieManager.Server.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public DateTime Showtime { get; set; }
        public double Price { get; set; }
        public int NumAvailible { get; set; }

        // Here for the EF core link
        public Movie Movie { get; set; }
    }
}
