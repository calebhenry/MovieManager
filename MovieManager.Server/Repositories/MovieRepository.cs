using MovieManager.Server.Models;
using System;

namespace MovieManager.Server.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private List<Movie> movies;
        private List<Cart> carts;
        private List<Ticket> tickets;
        private List<User> users;

        public MovieRepository()
        {
            movies = new List<Movie>();
            carts = new List<Cart>();
            tickets = new List<Ticket>();
            users = new List<User>();

            Movie movie1 = new Movie()
            {
                Id = 1,
                Name = "Grand Budapest Hotel",
                Description = "It's about a big hotel.",
                Tickets = new()
            };

            List<Ticket> ticket1 = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = 1,
                    MovieId = 1,
                    Showtime = DateTime.UtcNow,
                    Price = 2.50,
                    NumAvailible = 20,
                },
                new Ticket()
                {
                    Id = 2,
                    MovieId = 1,
                    Showtime = DateTime.UtcNow.AddHours(-2),
                    Price = 2.0,
                    NumAvailible = 25,
                }
            };

            movie1.Tickets = ticket1;

            Movie movie2 = new Movie()
            {
                Id = 2,
                Name = "Mini Budapest Hotel",
                Description = "It's about a very small hotel.",
                Tickets = new()
            };

            List<Ticket> ticket2 = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = 1,
                    MovieId = 2,
                    Showtime = DateTime.UtcNow,
                    Price = 2.50,
                    NumAvailible = 20,
                },
                new Ticket()
                {
                    Id = 2,
                    MovieId = 2,
                    Showtime = DateTime.UtcNow.AddHours(-2),
                    Price = 2.0,
                    NumAvailible = 25,
                }
            };

            movie2.Tickets = ticket2;
            tickets.Add(ticket2[0]);
            tickets.Add(ticket2[1]);
            tickets.Add(ticket1[0]);
            tickets.Add(ticket1[1]);
            movies.Add(movie1);
            movies.Add(movie2);
            Cart cart1 = new Cart();
            cart1.Id = 0;
            cart1.Tickets = new List<Ticket>() { ticket1[0], ticket2[0] };
            Cart cart2 = new Cart();
            cart2.Id = 0;
            cart2.Tickets = new List<Ticket>() { ticket1[1], ticket2[1] };
            carts.Add(cart1);
            carts.Add(cart2);

            var user1 = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL };
            var user2 = new User { Id = 2, Username = "Username 2", Password = "Password 2", Name = "Name 2", Gender = Gender.FEMALE, Age = 40, Email = "Email 2", PhoneNumber = "PhoneNumber 2", Preference = Preference.PHONE };
            users.Add(user1);
            users.Add(user2);
        }

        public List<Movie> GetMovies()
        {
            return movies;
        }

        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
        }

        public void RemoveMovie(Movie movie)
        {
            movies.Remove(movie);
        }

        public List<Ticket> GetTickets()
        {
            return tickets;
        }

        public void AddCart(Cart cart)
        {
            carts.Add(cart);
        }

        public void AddTicket(Ticket ticket)
        {
            tickets.Add(ticket);
        }

        public void RemoveTicket(Ticket ticket)
        {
            tickets.Remove(ticket);
        }

        public List<Cart> GetCarts()
        {
            return carts;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public User? GetUser(string username, string password)
        {
            User? user = users.FirstOrDefault(user => user.Username == username);

            if(user != null && user.Password != password) 
            {
                user = null;
            }

            return user;
        }

        public User UpdateUser(UpdatedUser updatedUser)
        {
            User user = users.First(user => user.Id == updatedUser.Id);

            if(!string.IsNullOrEmpty(updatedUser.Name)) 
            {
                user.Name = updatedUser.Name;
            }
            if(!string.IsNullOrEmpty(updatedUser.Email)) 
            {
                user.Email = updatedUser.Email;
            }
            if(!string.IsNullOrEmpty(updatedUser.PhoneNumber)) 
            {
                user.PhoneNumber = updatedUser.PhoneNumber;
            }
            if(updatedUser.Preference != null) 
            {
                user.Preference = updatedUser.Preference ?? user.Preference;
            }

            return user;
        }

        public void RemoveUser(User user)
        {
            users.Remove(user);
        }
    }
}
