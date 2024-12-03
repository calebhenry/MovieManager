using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieManager.Server.Models;
using MovieManager.Server.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MovieManager.Server.Services
{
    public class MovieService : IMovieService
    {
        private IMovieRepository movieRepository;

        public MovieService(IMovieRepository repository)
        {
            movieRepository = repository;
        }

        public List<Movie> GetMovies()
        {
            return movieRepository.GetMovies();
        }
        public Movie? GetMovieById(int id)
        {
            return movieRepository.GetMovieById(id);
        }

        public bool Liked(int userId, int reviewId)
        {
            return movieRepository.Liked(userId, reviewId);
        }
        public bool AddLike(int userId, int reviewId)
        {
            return movieRepository.AddLike(userId, reviewId);
        }

        public void AddMovie(Movie movie)
        {
            movieRepository.AddMovie(movie);
        }

        public void RemoveTicket(Ticket ticket)
        {
            movieRepository.RemoveTicket(ticket);
        }

        public void AddCart(Cart cart)
        {
            movieRepository.AddCart(cart);
        }

        /// <summary>
        /// Attempts to remove a ticket from a cart.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to remove.</param>
        /// <param name="cartId">The ID of the cart to remove the ticket from.</param>
        /// <returns>The cart with the specified ID if it exists. Null otherwise.</returns>
        public Cart? RemoveTicketFromCart(int ticketId, int cartId)
        {
            foreach (var cart in movieRepository.GetCarts())
            {
                if (cart.Id == cartId)
                {
                    foreach (var ticket in cart.Tickets)
                    {
                        if (ticket.TicketId == ticketId)
                        {
                            cart.Tickets.Remove(ticket);
                            movieRepository.UpdateCart(cart);
                            return cart;
                        }
                    }
                    return cart;
                }
            }
            return null;
        }
        public int AddReview(Review review)
        {
            return movieRepository.AddReview(review);
        }

        public void AddTicket(Ticket ticket)
        {
            movieRepository.AddTicket(ticket);
        }

        /// <summary>
        /// Attempts to add the specified number of the specifid ticket
        /// to the specified cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart to attempt to add ticket(s) to.</param>
        /// <param name="ticketId">The ID of the ticket to attempt to add to cart.</param>
        /// <param name="quantity">The quantity of the ticket to add to the cart.</param>
        /// <returns>Whether the tickets were sucessfully added to cart.</returns>
        public bool AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            var carts = (from i in movieRepository.GetCarts() where i.Id == cartId select i).ToArray();
            if (carts.Length == 0)
            {
                return false; // no carts with that cartId
            }
            Cart cart = carts.First();
            var tickets = (from i in movieRepository.GetTickets() where i.Id == ticketId select i).ToList();
            if (tickets.Count == 0)
            {
                return false; // no tickets with that ticketId
            }
            Ticket ticket = tickets.First();
            // get the total number of that ticket in all carts
            var ticketsInCarts = movieRepository.GetCarts().SelectMany(c => c.Tickets)
                .Where(t => t.TicketId == ticket.Id).Sum(item => item.Quantity);
            if (ticket.NumAvailible < quantity + ticketsInCarts)
            {
                return false; // not enough tickets available to add that quantity 
            }
            if (!cart.Tickets.Exists(t => t.TicketId == ticket.Id))
            { // cart does not already have that ticket
                var newCartItem = new CartItem
                {
                    Id = cart.Tickets.Count,
                    CartId = cart.Id,
                    TicketId = ticket.Id,
                    Quantity = 0,
                    Cart = cart,
                    Ticket = ticket
                }; // add that ticket to the cart
                cart.Tickets.Add(newCartItem);
            } // update ticket quanity in cart
            var cartItem = cart.Tickets.First(t => t.TicketId == ticket.Id);
            cartItem.Quantity += quantity;
            if (cartItem.Quantity < 0)
            {
                cartItem.Quantity = 0;
                return false;
            }
            movieRepository.UpdateCart(cart);
            return true;
        }

        public bool RemoveMovie(Movie movie)
        {
            return movieRepository.RemoveMovie(movie);
        }

        public User AddUser(User user)
        {
            user.Id = movieRepository.GetUsers()?.Count ?? 0;
            movieRepository.AddUser(user);
            return user;
        }

        public User? GetUser(string username, string password)
        {
            return movieRepository.GetUser(username, password);
        }

        public User UpdateUser(UpdatedUser updatedUser)
        {
            return movieRepository.UpdateUser(updatedUser);
        }

        public void RemoveUser(User user)
        {
            movieRepository.RemoveUser(user);
        }

        public void ProcessPayment(int cartId, string streetAddress, string city, string state, string zipCode, string cardNumber, string exp, string cardholderName, string cvc)
        {
            string[] streetAddressParts = streetAddress.Split(' ');
            if (!int.TryParse(streetAddressParts[0], out _))
            {
                throw new ArgumentException("Invalid street number. Payment could not be processed.");
            }

            if (streetAddressParts.Length < 3)
            {
                throw new ArgumentException("Invalid street address. Payment could not be processed.");
            }

            if (city.Any(char.IsDigit))
            {
                throw new ArgumentException("Invalid city. Payment could not be processed.");
            }

            if (state.Length != 2 || state.Any(char.IsDigit))
            {
                throw new ArgumentException("Invalid state abbreviation. Payment could not be processed.");
            }

            if (zipCode.Length != 5 || !int.TryParse(zipCode, out _))
            {
                throw new ArgumentException("Invalid zip code. Payment could not be processed.");
            }

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(exp) || string.IsNullOrEmpty(cardholderName) || string.IsNullOrEmpty(cvc))
            {
                throw new ArgumentException("Each field needs to be filled out. Payment could not be processed.");
            }

            if (cardNumber.Length != 16)
            {
                throw new ArgumentException("Card number is invalid. Payment could not be processed.");
            }

            if (cvc.Length != 3)
            {
                throw new ArgumentException("CVC is invalid. Payment could not be processed.");
            }

            if (exp.Length != 7)
            {
                throw new ArgumentException("Expiration date is invalid. Payment could not be processed.");
            }

            int month = int.Parse(exp[0..2]);
            int year = int.Parse(exp[3..7]);

            if (year < DateTime.Now.Year || (year == DateTime.Now.Year && month < DateTime.Now.Month))
            {
                throw new ArgumentException("Card is expired. Payment could not be processed.");
            }

            var cart = movieRepository.GetCarts().FirstOrDefault(c => c.Id == cartId);
            foreach (var item in cart?.Tickets ?? new())
            {
                var ticket = movieRepository.GetTickets().FirstOrDefault(t => t.MovieId == item.Ticket.MovieId && t.Id == item.TicketId);
                ticket.NumAvailible -= item.Quantity;
            }
            cart?.Tickets.Clear();
        }
        public List<Ticket> GetAllTickets()
        {
            return movieRepository.GetTickets();
        }
        public IEnumerable<Ticket> GetTickets(int movieId)
        {
            var movie = movieRepository.GetMovies().FirstOrDefault(m => m.Id == movieId);
            return movie?.Tickets ?? Enumerable.Empty<Ticket>();
        }

        public Cart GetCart(int? cartId)
        {
            var cart = movieRepository.GetCarts().FirstOrDefault(c => c.Id == cartId);
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = movieRepository.GetCarts().Count,
                    Tickets = new List<CartItem>()
                };
                movieRepository.AddCart(cart);
            }
            return cart;
        }

        public Review EditReview(UpdatedReview updatedReview)
        {
            return movieRepository.EditReview(updatedReview);
        }

        public Ticket EditTickets(UpdatedTicket updatedTicket)
        {
            return movieRepository.EditTickets(updatedTicket);
        }

        public Movie EditMovie(UpdatedMovie updatedMovie)
        {
            Console.WriteLine("Updating movie");
            return movieRepository.EditMovie(updatedMovie);
        }
        
        public List<Review> GetReviews(int movieId)
        {
            return movieRepository.GetReviews(movieId);
        }
        
        public void AddTicketsToMovie(Ticket ticket)
        {
            movieRepository.AddTicketsToMovie(ticket);
        }
        public void RemoveTicketFromMovie (int movieId, int NumAvailible){
            var movie = movieRepository.GetMovies().FirstOrDefault(m => m.Id == movieId);
            foreach (var ticket in movie.Tickets.ToList()){
                    movie.Tickets.Remove(ticket);
            }
             NumAvailible--;
        }
        public void RemoveTicketsFromMovie(int movieId, int numAvailable)
        {
            return movieRepository.RemoveTicketsFromMovie(movieId, numTickets);
        }
        public void RemoveReview(Review review)
        {
            movieRepository.RemoveReview(review);
        }
    }
}
