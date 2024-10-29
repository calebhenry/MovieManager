using MovieManager.Server.Models;
using MovieManager.Server.Repositories;

namespace MovieManager.Server.Services
{
    public class MovieService : IMovieService
    {
        private List<Movie> Movies;
        private IMovieRepository MovieRepository;

        public MovieService(IMovieRepository repository)
        {
            Movies = new List<Movie>();
            MovieRepository = repository;
        }

        public List<Movie> GetMovies()
        {
            return MovieRepository.GetMovies();
        }

        public void AddMovie(Movie movie)
        {
            MovieRepository.AddMovie(movie);
        }

        public void RemoveMovie(Movie movie)
        {
            MovieRepository.RemoveMovie(movie);
        }

        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc)
        {
            MovieRepository.ProcessPayment(cartId, cardNumber, exp, cardholderName, cvc);
        }
    }
}
