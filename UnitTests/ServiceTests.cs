using MovieManager.Server.Services;
using MovieManager.Server.Repositories;
using MovieManager.Server.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void GetUser_ReturnsSingleUser()
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL },
                new User { Id = 2, Username = "Username 2", Password = "Password 2", Name = "Name 2", Gender = Gender.FEMALE, Age = 40, Email = "Email 2", PhoneNumber = "PhoneNumber 2", Preference = Preference.PHONE }
            };
            _mockRepository.Setup(repo => repo.GetUser("Username 1", "Password 1")).Returns(users[0]);
            var result = _movieService.GetUser("Username 1", "Password 1");
            Assert.AreEqual(users[0], result);
        }

        [Test]
        public void AddUser_CallsRepositoryAddUser()
        {
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL };
            _movieService.AddUser(user);
            _mockRepository.Verify(repo => repo.AddUser(user), Times.Once);
        }

        [Test]
        public void UpdateUser_CallsRepositoryUpdateUser()
        {
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL };
            var updatedUser = new UpdatedUser { Id = 1, Name = "Name 3", Email = "Email 3", PhoneNumber = "PhoneNumber 3", Preference = Preference.PHONE };
            var expectedUser = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 3", Gender = Gender.MALE, Age = 40, Email = "Email 3", PhoneNumber = "PhoneNumber 3", Preference = Preference.PHONE };
            _mockRepository.Setup(repo => repo.UpdateUser(updatedUser)).Returns(expectedUser);
            var result = _movieService.UpdateUser(updatedUser);

            Assert.AreEqual(updatedUser.Name, result.Name);
            Assert.AreEqual(updatedUser.Email, result.Email);
            Assert.AreEqual(updatedUser.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(updatedUser.Preference, result.Preference);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Password, result.Password);
            Assert.AreEqual(user.Age, result.Age);

            _mockRepository.Verify(repo => repo.UpdateUser(updatedUser), Times.Once);
        }

        [Test]
        public void RemoveUser_CallsRepositoryRemoveUser()
        {
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL };
            _movieService.RemoveUser(user);
            _mockRepository.Verify(repo => repo.RemoveUser(user), Times.Once);
        }

        [Test]
        public void ProcessPayment_CallsRepositoryProcessPayment()
        {
            int cartId = 1;
            string cardNumber = "1234567890123456";
            string exp = "12/23";
            string cardholderName = "John Doe";
            string cvc = "123";

            _movieService.ProcessPayment(cartId, cardNumber, exp, cardholderName, cvc);
            _mockRepository.Verify(repo => repo.ProcessPayment(cartId, cardNumber, exp, cardholderName, cvc), Times.Once);
        }

        [Test]
        public void AddCart_CallsRepositoryAddCart()
        {
            var cart = new Cart { Id = 1, Tickets = new List<CartItem>() };
            _movieService.AddCart(cart);
            _mockRepository.Verify(repo => repo.AddCart(cart), Times.Once);
        }

        [Test]
        public void RemoveTicket_CallsRepositoryRemoveTicket()
        {
            var ticket = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailible = 20 };
            _movieService.RemoveTicket(ticket);
            _mockRepository.Verify(repo => repo.RemoveTicket(ticket), Times.Once);
        }

        [Test]
        public void RemoveTicketFromCart_RemovesTicketAndReturnsUpdatedCart()
        {
            var cart = new Cart
            {
                Id = 1,
                Tickets = new List<CartItem>
                {
                    new CartItem { Id = 1, TicketId = 1, Quantity = 2, Ticket = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailible = 20 } }
                }
            };

            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart> { cart });

            var result = _movieService.RemoveTicket(1, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Tickets.Count);
        }

        [Test]
        public void AddTicketToCart_AddsTicketAndReturnsTrue()
        {
            var cart = new Cart { Id = 1, Tickets = new List<CartItem>() };
            var ticket = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailible = 20 };

            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart> { cart });
            _mockRepository.Setup(repo => repo.GetTickets()).Returns(new List<Ticket> { ticket });

            var result = _movieService.AddTicketToCart(1, 1, 2);

            Assert.IsTrue(result);
            Assert.AreEqual(1, cart.Tickets.Count);
            Assert.AreEqual(2, cart.Tickets.First().Quantity);
        }

        [Test]
        public void AddTicketToCart_ReturnsFalseIfCartNotFound()
        {
            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart>());

            var result = _movieService.AddTicketToCart(1, 1, 2);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddTicketToCart_ReturnsFalseIfTicketNotFound()
        {
            var cart = new Cart { Id = 1, Tickets = new List<CartItem>() };

            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart> { cart });
            _mockRepository.Setup(repo => repo.GetTickets()).Returns(new List<Ticket>());

            var result = _movieService.AddTicketToCart(1, 1, 2);

            Assert.IsFalse(result);
        }

        [Test]
        public void AddTicketToCart_ReturnsFalseIfNotEnoughTicketsAvailable()
        {
            var cart = new Cart { Id = 1, Tickets = new List<CartItem>() };
            var ticket = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailible = 1 };

            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart> { cart });
            _mockRepository.Setup(repo => repo.GetTickets()).Returns(new List<Ticket> { ticket });

            var result = _movieService.AddTicketToCart(1, 1, 2);

            Assert.IsFalse(result);
        }
    }
}
