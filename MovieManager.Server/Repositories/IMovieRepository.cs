using MovieManager.Server.Models;

namespace MovieManager.Server.Repositories
{
    public interface IMovieRepository
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        void AddTicket(Ticket ticket);
        void AddUser(User user);
        List<Cart> GetCarts();
        List<Movie> GetMovies();
        List<Ticket> GetTickets();
        List<User> GetUsers();
        Showtime GetShowtime(int movieId, DateTime showtime);
        User? GetUser(string username, string password);
        User UpdateUser(UpdatedUser updatedUser);
        void RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        void RemoveUser(User user);
        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc);
        public bool ProcessTickets (int cartId, List<Ticket> tickets, MovieRepository movieRepo);
    }
}