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
        private List<Showtime> showtimes;
    

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
            movies.Add(movie1);
            movies.Add(movie2);

            Cart cart1 = new Cart();

            cart1.Id = 0;
            List<CartItem> cartItems1 = new List<CartItem>();
            for (int i = 0; i < ticket1.Count; i++)
            {
                var ticket = ticket1[i];
                cartItems1.Add(
                    new CartItem()
                    {
                        Id = i,
                        CartId = 0,
                        TicketId = ticket.Id,
                        Quantity = 5,
                        Cart = cart1,
                        Ticket = ticket

                    }
                );
            }

            cart1.Tickets = cartItems1;

            Cart cart2 = new Cart();
            cart2.Id = 1;

            List<CartItem> cartItems2 = new List<CartItem>();
            for (int i = 0; i < ticket2.Count; i++)
            {
                var ticket = ticket2[i];
                cartItems2.Add(
                    new CartItem()
                    {
                        Id = i,
                        CartId = 0,
                        TicketId = ticket.Id,
                        Quantity = 4,
                        Cart = cart1,
                        Ticket = ticket

                    }
                );
            }

            cart2.Tickets = cartItems2;
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

        public Movie GetMovieById(int id)
        {
            return movies.SingleOrDefault(m => m.Id == id);
        }


        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
        }

        public void RemoveMovie(Movie movie)
        {
            movies.Remove(movie);
        }
        public void RemoveCart(Cart cart){
            carts.Remove(cart);
        }
        public List<Ticket> GetTickets()
        {
            return movies.SelectMany(m => m.Tickets).ToList();
        }

        public Showtime GetShowtime(int movieId, DateTime showtime)
        {
            Showtime time = showtimes.First(s => s.MovieId == movieId && s.Time == showtime);
            return time;
        }
        public Cart GetCartById(int cartId){
            var cart = carts.First(c => c.Id == cartId);
            return cart;
        }
        public void AddCart(Cart cart)
        {
            carts.Add(cart);
        }

        public void AddTicket(Ticket ticket)
        {
            movies.First(m => m.Id == ticket.MovieId).Tickets.Add(ticket);
        }

        public void RemoveTicket(Ticket ticket)
        {
            movies.First(m => m.Id == ticket.MovieId).Tickets.Remove(ticket);
        }

        public List<Cart> GetCarts()
        {
            return carts;
        }

        public List<User> GetUsers()
        {
            return users ?? new List<User>();
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

        // Process Payment method verifies whether or not input from user is valid
        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc, List<Ticket> tickets)
        {
            if(string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(exp) || string.IsNullOrEmpty(cardholderName) || string.IsNullOrEmpty(cvc))
            {
                throw new ArgumentException("Each field needs to be filled out. Payment could not be processed.");
            }

            if(cardNumber.Length != 16)
            {
                throw new ArgumentException("Card number is invalid. Payment could not be processed.");
            }

            if(cvc.Length != 3)
            {
                throw new ArgumentException("CVC is invalid. Payment could not be processed.");
            }

            if(exp.Length != 6)
            {
                throw new ArgumentException("Expiration date is invalid. Payment could not be processed.");
            }

            int month = int.Parse(exp[0..2]);
            int year = int.Parse(exp[2..6]);

            if(year < DateTime.Now.Year || (year == DateTime.Now.Year && month < DateTime.Now.Month))
            {
                throw new ArgumentException("Card is expired. Payment could not be processed.");
            }
           ProcessTickets(cartId, tickets);
        } 
           
            // Update quantity of tickets avaialable associated with showtime after payment is processed
            public bool ProcessTickets(int cartId, List<Ticket> tickets)
            {
            // Retrieve the cart based on the cartId
            Cart cart = GetCartById(cartId);
            if (cart == null)
            {
                throw new ArgumentException("Invalid cart ID.");
            }
            // Loop through each CartItem in the cart
            foreach (var cartItem in cart.Tickets)
            {
                // Find the ticket in the list of tickets from the movie repository based on the CartItem ticket id
                Ticket ticket = tickets.First(t => t.Id == cartItem.TicketId);
        
                if (ticket == null)
                {
                    return false;
                }
                // Find the showtime associated with this ticket
                Showtime showtime = GetShowtime(ticket.MovieId, ticket.Showtime);
                if (showtime == null || showtime.NumAvailable <= 0)
                {
                    // If no available tickets left for this showtime, return false
                    return false;
                }
            // Decrement the number of available tickets
            showtime.NumAvailable--;
        }   
        // TO-DO finish removing cart items from cart
        // cart.RemoveCart();
        return true;
    } 
    }
}

