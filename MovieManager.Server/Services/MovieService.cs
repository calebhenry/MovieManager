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

        public Cart? RemoveTicket(int ticketId, int cartId)
        {
            foreach (var cart in movieRepository.GetCarts())
            {
                if (cart.Id == cartId)
                {
                    foreach (var ticket in cart.Tickets)
                    {
                        if (ticket.Id == ticketId)
                        {
                            cart.Tickets.Remove(ticket);
                            return cart;
                        }
                    }
                    return cart;
                }
            }
            return null;
        }

        public void RemoveMovie(Movie movie)
        {
            movieRepository.RemoveMovie(movie);
        }
        public IEnumerable<Ticket> GetTickets(int movieId)
        {
            var movie = movieRepository.GetMovies().FirstOrDefault(m => m.Id == movieId);
            return movie?.Tickets ?? Enumerable.Empty<Ticket>();
        }
        public Cart GetCart(int cartId)
        {
            var cart = movieRepository.GetCarts().FirstOrDefault(c => c.Id == cartId);

            if (cart == null)
            {
                cart = new Cart { Id = cartId, Tickets = new List<Ticket>() };
                movieRepository.AddCart(cart);
            }
            return cart;
        }
    }
}
