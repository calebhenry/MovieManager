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
    }
}
