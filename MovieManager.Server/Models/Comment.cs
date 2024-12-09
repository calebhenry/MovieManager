using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
    }
}