using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public class MovieService : IMovieService
    {
        private List<Movie> movies;

        public MovieService()
        {
            movies = new List<Movie>();
        }

        public List<Movie> GetMovies()
        {
            return movies;
        }

        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
        }
    }
}
