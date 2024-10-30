using System.ComponentModel;

namespace MovieManager.Server.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
