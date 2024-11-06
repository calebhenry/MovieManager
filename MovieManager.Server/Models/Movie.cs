using System.ComponentModel;

namespace MovieManager.Server.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; } = Genre.ACTION;
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

    public enum Genre
    {
        ACTION,
        COMEDY,
        DRAMA,
        HORROR,
        ROMANCE,
        THRILLER
    }
}
