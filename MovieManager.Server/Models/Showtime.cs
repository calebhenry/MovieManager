using System.ComponentModel;

namespace MovieManager.Server.Models
{
    public class Showtime
    {
        public int NumAvailable { get; set; }
        public DateTime Time { get; set;}
        public int MovieId { get; set;}
    }
}