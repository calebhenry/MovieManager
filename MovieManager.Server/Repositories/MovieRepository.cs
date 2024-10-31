using MovieManager.Server.Models;
using System;

namespace MovieManager.Server.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private List<Movie> movies;
        private List<Cart> carts;
        //private List<Ticket> tickets;

        public MovieRepository()
        {
            movies = new List<Movie>();
            carts = new List<Cart>();
            //tickets = new List<Ticket>();
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
            return movies.SelectMany(m => m.Tickets).ToList();
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

        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc)
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

            //update ticket quantities- has to be done after Dylan's PR since he's changing the cart class

            //empty cart- has to be done after Dylan's PR since he's changing the cart class
        }
    }
}
