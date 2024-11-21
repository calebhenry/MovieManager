using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManager.Server.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; } = Genre.ACTION;
        [InverseProperty("Movie")]
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        [InverseProperty("Movie")]
        public List<Review> Reviews { get; set; } = new List<Review>();
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
