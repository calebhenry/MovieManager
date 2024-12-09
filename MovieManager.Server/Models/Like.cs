using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int UserId { get; set; }
    }
}
