using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    /// <summary>
    /// Represents a Movie.
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genre Genre { get; set; } = Genre.ACTION;
        public int AgeRating { get; set; } = 13;
        public double ReviewScore
        {
            get
            {
                if (Reviews.Count == 0)
                    return -1;
                return (double) Reviews.Sum(r => r.Rating) / Reviews.Count;
            }
        }
        [InverseProperty("Movie")]
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        [InverseProperty("Movie")]
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
    /// <summary>
    /// When a movie information is updated.
    /// </summary>
    public class UpdatedMovie 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Genre Genre { get; set; } = Genre.ACTION;
        public int AgeRating { get; set; } = 13;
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
