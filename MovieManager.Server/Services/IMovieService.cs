using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicketToCart(int cartId, int ticketId, int quantity);
        User AddUser(User user);
        List<Movie> GetMovies();
        Movie GetMovieById(int id);
        User? GetUser(string username, string password);
        User UpdateUser(UpdatedUser updatedUser);
        bool RemoveMovie(Movie movie);
        Cart? RemoveTicketFromCart(int ticketId, int cartId);
        void RemoveTicket(Ticket ticket);
        void RemoveUser(User user);
        void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc);
        IEnumerable<Ticket> GetTickets(int movieId);
        Cart GetCart(int? cartId);
    }
}