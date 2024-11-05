using System.ComponentModel;

namespace MovieManager.Server.Models
{
    public class Showtime
    {
        public int NumAvailible { get; set; }
        public DateTime Showtime { get; set;}
        public int MovieId { get; set;}
    }
}