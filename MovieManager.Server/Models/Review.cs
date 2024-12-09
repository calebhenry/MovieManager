using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    /// <summary>
    /// Review of a movie. 
    /// </summary>
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int LikeCount { get; set; }
        public bool Anonymous { get; set; }

        // Here for the EF core link
        [JsonIgnore]
        public Movie Movie { get; set; } = null!;
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
    /// <summary>
    /// Review of a user. 
    /// </summary>
    public class ReviewDTO
    {
        public ReviewDTO(Review review, string userName)
        {
            Id = review.Id;
            MovieId = review.MovieId;
            UserId = review.UserId;
            PostDate = review.PostDate;
            Comment = review.Comment;
            Rating = review.Rating;
            LikeCount = review.LikeCount;
            Anonymous = review.Anonymous;
            Username = review.Anonymous ? "Anon" : userName;
        }
        public Review toReview()
        {
            Review rtn = new Review();
            rtn.Id = Id;
            rtn.MovieId = MovieId;
            rtn.UserId = UserId;
            rtn.PostDate = PostDate;
            rtn.Comment = Comment;
            rtn.Rating = Rating;
            rtn.LikeCount = LikeCount;
            rtn.Anonymous = Anonymous;
            return rtn;
        }
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int LikeCount { get; set; }
        public bool Anonymous { get; set; }
        public string Username { get; set; }
    }
    /// <summary>
    /// When a review is updated/changed.
    /// </summary>
    public class UpdatedReview
    {
        public int Id { get; set; }
        public DateTime PostDate { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public bool Anonymous { get; set; }
    }
}
