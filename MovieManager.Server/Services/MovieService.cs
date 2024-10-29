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
                        if (ticket.Key.Id == ticketId)
                        {
                            cart.Tickets.Remove(ticket.Key);
                            return cart;
                        }
                    }
                    return cart;
                }
            }
            return null;
        }

        public bool AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            var carts = (from i in movieRepository.GetCarts() where i.Id == cartId select i).ToList();
            if (carts.Count == 0)
            {
                return false;
            }
            Cart cart = carts.First();
            var tickets = (from i in movieRepository.GetTickets() where i.Id == ticketId select i).ToList();
            if (tickets.Count == 0)
            {
                return false;
            }
            Ticket ticket = tickets.First();
            // assuming we decrement numAvailable only after they checkout
            var ticketsInCarts = (from i in movieRepository.GetCarts() where i.Tickets.ContainsKey(ticket) select i.Tickets[ticket]).Sum();
            if (ticket.NumAvailible < quantity + ticketsInCarts)
            {
                return false;
            }
            if (!cart.Tickets.ContainsKey(ticket))
            {
                cart.Tickets[ticket] = 0;
            }
            cart.Tickets[ticket] += quantity;
            return true;
        }

        public void RemoveMovie(Movie movie)
        {
            movieRepository.RemoveMovie(movie);
        }
    }
}
