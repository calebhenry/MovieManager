using System.ComponentModel;

namespace MovieManager.Server.Models
{
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
    }

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
}