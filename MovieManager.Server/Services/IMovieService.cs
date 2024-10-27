using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        public List<Movie> GetMovies();
        public void AddMovie(Movie movie);
        public void RemoveMovie(Movie movie);
    }
}
