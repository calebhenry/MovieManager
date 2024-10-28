using MovieManager.Server.Services;
using MovieManager.Server.Repositories;

namespace UnitTests
{
    public class Tests
    {
        public MovieService Service;

        [SetUp]
        public void Setup()
        {
            Service = new MovieService(new MovieRepository());
        }

        [Test]
        public void Test1()
        {
            var thing = Service.GetMovies().Count;
            Assert.That(Service.GetMovies().Count == 2);
        }
    }
}