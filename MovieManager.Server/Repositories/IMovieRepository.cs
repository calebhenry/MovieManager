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
        Movie GetMovieById(int id);
        List<User> GetUsers();
        User? GetUser(string username, string password);
        User UpdateUser(UpdatedUser updatedUser);
        bool RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        void RemoveUser(User user);
    }
}