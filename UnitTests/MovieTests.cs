using MovieManager.Server.Services;
using MovieManager.Server.Repositories;
using MovieManager.Server.Models;

namespace UnitTests
{
    public class MovieTests
    {
        public IMovieService Service;

        [SetUp]
        public void Setup()
        {
            Service = new MovieService(new MovieRepository());
        }

        [Test]
        public void GetMoviesCorrectNumber()
        {
            var movies = Service.GetMovies().Count;
            Assert.That(Service.GetMovies().Count == 2);
        }

        [Test]
        public void AddMovieCorrectNumber()
        {
            var movies = Service.GetMovies().Count;
            Service.AddMovie(new Movie());
            Assert.That(Service.GetMovies().Count == movies + 1);
        }

        [Test]
        public void RemoveMovieCorrectNumber()
        {
            var movies = Service.GetMovies().Count;
            Service.RemoveMovie(new Movie() { Id = 1});
            Assert.That(Service.GetMovies().Count == movies - 1);
        }
    }
}