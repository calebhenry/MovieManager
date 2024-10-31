using MovieManager.Server.Services;
using MovieManager.Server.Repositories;
using MovieManager.Server.Models;
using Moq;

namespace UnitTests
{
    public class ServiceTests
    {
        private Mock<IMovieRepository> _mockRepository;
        private MovieService _movieService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IMovieRepository>();
            _movieService = new MovieService(_mockRepository.Object);
        }

        [Test]
        public void GetMovies_ReturnsListOfMovies()
        {
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" },
                new Movie { Id = 2, Name = "Movie 2", Description = "Description 2" }
            };
            _mockRepository.Setup(repo => repo.GetMovies()).Returns(movies);
            var result = _movieService.GetMovies();
            Assert.AreEqual(movies, result);
        }

        [Test]
        public void AddMovie_CallsRepositoryAddMovie()
        {
            var movie = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            _movieService.AddMovie(movie);
            _mockRepository.Verify(repo => repo.AddMovie(movie), Times.Once);
        }

        [Test]
        public void RemoveMovie_CallsRepositoryRemoveMovie()
        {
            var movie = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            _movieService.RemoveMovie(movie);
            _mockRepository.Verify(repo => repo.RemoveMovie(movie), Times.Once);
        }

        [Test]
        public void ProcessPayment_CallsRepositoryProcessPayment()
        {
            var movie1 = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            var movie2 = new Movie { Id = 2, Name = "Movie 2", Description = "Description 2" }
            var ticket1 = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailable = 20 }
            var ticket2 = new Tikcet { Id = 2, MovieId = 2, Showtime = DateTime.UtcNow.AddHours(-2), Price = 2.0, NumAvailible = 25 }

            //have to finish the rest of the test after Dylan's PR gets merged due to changes in the cart class
        }
    }
}