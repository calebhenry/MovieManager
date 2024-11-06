﻿using System.Text.Json.Serialization;

namespace MovieManager.Server.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        // Here for the EF core link
        [JsonIgnore]
        public Movie Movie { get; set; } = null!;
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}