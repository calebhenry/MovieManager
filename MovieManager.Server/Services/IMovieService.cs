using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicketToCart(int cartId, int ticketId, int quantity);
        User AddUser(User user);
        Cart GetCart(int? cartId);
        Movie? GetMovieById(int id);
        void AddTicket(Ticket ticket);
        bool AddReview(Review review);
        List<Movie> GetMovies();
        IEnumerable<Ticket> GetTickets(int movieId);
        User? GetUser(string username, string password);
        void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc);
        bool RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        Cart? RemoveTicketFromCart(int ticketId, int cartId);
        void RemoveUser(User user);
        User UpdateUser(UpdatedUser updatedUser);
        Review EditReview(int currentUserId, UpdatedReview updatedReview);
        public Ticket EditTickets(int movieId, UpdatedTicket updatedTicket);
        public Movie EditMovie(int movieId, UpdatedMovie updatedMovie);
        List<Review> GetReviews(int movieId);
    }
}