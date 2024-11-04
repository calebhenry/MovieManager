using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        void AddUser(User user);
        List<Movie> GetMovies();
        User? GetUser(string username, string password);
        User UpdateUser(UpdatedUser updatedUser);
        void RemoveMovie(Movie movie);
        Cart? RemoveTicket(int ticketId, int cartId);
        void RemoveTicket(Ticket ticket);
        void RemoveUser(User user);
    }
}