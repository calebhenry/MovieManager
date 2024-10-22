using MovieManager.Server.Models;

namespace MovieManager.Server.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetMovies();
        public void AddMovie(Movie movie);
    }
}
