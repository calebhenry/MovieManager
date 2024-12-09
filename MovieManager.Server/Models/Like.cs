using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    /// <summary>
    /// Like a movie.
    /// </summary>
    public class Like
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int UserId { get; set; }
    }
}
