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
