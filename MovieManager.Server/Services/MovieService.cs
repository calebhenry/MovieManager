using MovieManager.Server.Models;
using MovieManager.Server.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MovieManager.Server.Services
{
    public class MovieService : IMovieService
    {
        private IMovieRepository movieRepository;
        private User? currentUser;

        public MovieService(IMovieRepository repository)
        {
            movieRepository = repository;
        }

        public List<Movie> GetMovies()
        {
            return movieRepository.GetMovies();
        }
        public Movie GetMovieById(int id)
        {
            return movieRepository.GetMovieById(id);
        }

        public void AddMovie(Movie movie)
        {
            foreach(var ticket in movie.Tickets)
            {
                ticket.MovieId = movie.Id;
                ticket.Movie = movie;
            }
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

        public Cart? RemoveTicket(int ticketId, int cartId, int movieId)
        {
            var cart = movieRepository.GetCarts().Where(c => c.Id == cartId).FirstOrDefault();
            if (cart == null)
                return null;
            foreach (var ticket in cart.Tickets)
            {
                if (ticket.TicketId == ticketId && ticket.Ticket.MovieId == movieId)
                {
                    cart.Tickets.Remove(ticket);
                    return cart;
                }
            }
            return cart;
        }

        public bool AddTicketToCart(int cartId, int ticketId, int quantity, int movieId)
        {
            if (quantity < 0)
            {
                return false;
            }
            var carts = (from i in movieRepository.GetCarts() where i.Id == cartId select i).ToList();
            if (carts.Count == 0)
            {
                return false;
            }
            Cart cart = carts.First();
            if (cart.Tickets.FirstOrDefault(t => t.TicketId == ticketId && t.Ticket.MovieId == movieId) != null)
                cart.Tickets.FirstOrDefault(t => t.TicketId == ticketId && t.Ticket.MovieId == movieId).Quantity = 0;
            var tickets = (from i in movieRepository.GetTickets() where i.Id == ticketId && i.MovieId == movieId select i).ToList();
            if (tickets.Count == 0)
            {
                return false;
            }
            Ticket ticket = tickets.First();
            // assuming we decrement numAvailable only after they checkout
            var ticketsInCarts = movieRepository.GetCarts().SelectMany(c => c.Tickets).Where(t => t.TicketId == ticket.Id && t.Ticket.MovieId == movieId).Sum(item => item.Quantity);
            if (ticket.NumAvailible < quantity + Math.Max(ticketsInCarts - quantity, 0))
            {
                return false;
            }
            if (!cart.Tickets.Exists(t => t.TicketId == ticket.Id && t.Ticket.MovieId == movieId))
            {
                var cartItem = new CartItem
                {
                    Id = cart.Tickets.Count,
                    CartId = cart.Id,
                    TicketId = ticket.Id,
                    Quantity = 0,
                    Cart = cart,
                    Ticket = ticket
                };
                cart.Tickets.Add(cartItem);
            }
            cart.Tickets.First(t => t.TicketId == ticket.Id && t.Ticket.MovieId == movieId).Quantity += quantity;
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
            Console.WriteLine("Updating user");
            return movieRepository.UpdateUser(updatedUser);
        }

        public void RemoveUser(User user)
        {
            movieRepository.RemoveUser(user);
        }

        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc)
        {
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
    }
}
