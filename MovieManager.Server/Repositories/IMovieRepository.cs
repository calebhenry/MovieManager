using MovieManager.Server.Models;

namespace MovieManager.Server.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetMovies();
        public void AddMovie(Movie movie);
        public void RemoveMovie(Movie movie);
        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc);
    }
}
