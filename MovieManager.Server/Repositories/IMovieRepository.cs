using MovieManager.Server.Models;

namespace MovieManager.Server.Repositories
{
    public interface IMovieRepository
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        void AddTicket(Ticket ticket);
        List<Cart> GetCarts();
        List<Movie> GetMovies();
        List<Ticket> GetTickets();
        void RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc);
    }
}