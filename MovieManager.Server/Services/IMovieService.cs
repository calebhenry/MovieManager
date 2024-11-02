using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicketToCart(int cartId, int ticketId, int quantity);
        List<Movie> GetMovies();
        void RemoveMovie(Movie movie);
        Cart? RemoveTicket(int ticketId, int cartId);
        void RemoveTicket(Ticket ticket);
        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc);
        IEnumerable<Ticket> GetTickets(int movieId);
        Cart GetCart(int cartId);
    }
}