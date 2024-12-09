using MovieManager.Server.Models;

namespace MovieManager.Server.Repositories
{
    /// <summary>
    /// Interface for interacting with the Movie repository.
    /// Defines methods for managing movies, users, carts, tickets, and reviews.
    /// </summary>
    public interface IMovieRepository
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicket(Ticket ticket);
        void AddUser(User user);
        int AddReview(Review review);
        List<Cart> GetCarts();
        void UpdateCart(Cart cart);
        Movie? GetMovieById(int id);
        List<Movie> GetMovies();
        List<Ticket> GetAllTickets();
        List<Ticket> GetTickets();
        User? GetUser(string username, string password);
        List<User> GetUsers();
        bool RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        void RemoveUser(User user);
        User? UpdateUser(UpdatedUser updatedUser);
        Review? EditReview(UpdatedReview updatedReview);
        Ticket EditTickets(int movieId, UpdatedTicket updatedTicket);
        Movie? EditMovie(UpdatedMovie updatedMovie);
        List<Review> GetReviews(int movieId);
        void AddTicketsToMovie(Ticket ticket);
        bool Liked(int userId, int reviewId);
        bool AddLike(int userId, int reviewId);
        bool RemoveReview(Review review);
        bool RemoveTicketsFromMovie(int movieId, int numTickets);
        bool RemoveLike(int userId, int reviewId);
        List<Comment> GetComments(int reviewId);
        bool AddComment(Comment comment);
    }
}