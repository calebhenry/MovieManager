using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManager.Server.Models
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Preference Preference { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
        [InverseProperty("User")]
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
    /// <summary>
    /// When user information if changed/updated. 
    /// </summary>
    public class UpdatedUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Preference? Preference { get; set; }
    }

    public enum Gender
    {
        MALE,
        FEMALE,
        OTHER
    }

    public enum Preference
    {
        EMAIL,
        PHONE
    }

    public enum PermissionLevel
    {
        USER,
        ADMIN
    }
}