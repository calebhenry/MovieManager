using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public DateTime Showtime { get; set; }
        public double Price { get; set; }
        public int NumAvailible { get; set; }

        // Here for the EF core link
        [JsonIgnore]
        public Movie Movie { get; set; } = null!;
    }

    public class UpdatedTicket 
    {
        public int Id { get; set; }
        public int  MovieId { get; set; }
        public double? Price { get; set; }
        public int? NumAvailible { get; set; }
    }
}
