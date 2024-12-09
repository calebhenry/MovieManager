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

        #region Movies

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
            Assert.That(movies == result);
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
        public void GetMovieById_ReturnsCorrectMovie()
        {
            var movie = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            _mockRepository.Setup(repo => repo.GetMovieById(1)).Returns(movie);
            var result = _movieService.GetMovieById(1);
            Assert.That(movie == result);
        }

        #endregion

        #region Users

        [Test]
        public void GetUser_ReturnsSingleUser()
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL, PermissionLevel = PermissionLevel.USER },
                new User { Id = 2, Username = "Username 2", Password = "Password 2", Name = "Name 2", Gender = Gender.FEMALE, Age = 40, Email = "Email 2", PhoneNumber = "PhoneNumber 2", Preference = Preference.PHONE, PermissionLevel = PermissionLevel.USER }
            };
            _mockRepository.Setup(repo => repo.GetUser("Username 1", "Password 1")).Returns(users[0]);
            var result = _movieService.GetUser("Username 1", "Password 1");
            Assert.That(users[0] == result);
        }

        [Test]
        public void AddUser_CallsRepositoryAddUser()
        {
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL, PermissionLevel = PermissionLevel.USER };
            _movieService.AddUser(user);
            _mockRepository.Verify(repo => repo.AddUser(user), Times.Once);
        }

        [Test]
        public void UpdateUser_CallsRepositoryUpdateUser()
        {
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL, PermissionLevel = PermissionLevel.USER };
            var updatedUser = new UpdatedUser { Id = 1, Name = "Name 3", Email = "Email 3", PhoneNumber = "PhoneNumber 3", Preference = Preference.PHONE };
            var expectedUser = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 3", Gender = Gender.MALE, Age = 40, Email = "Email 3", PhoneNumber = "PhoneNumber 3", Preference = Preference.PHONE, PermissionLevel = PermissionLevel.USER };
            _mockRepository.Setup(repo => repo.UpdateUser(updatedUser)).Returns(expectedUser);
            var result = _movieService.UpdateUser(updatedUser);

            Assert.That(updatedUser.Name == result.Name);
            Assert.That(updatedUser.Email == result.Email);
            Assert.That(updatedUser.PhoneNumber == result.PhoneNumber);
            Assert.That(updatedUser.Preference == result.Preference);
            Assert.That(user.Username == result.Username);
            Assert.That(user.Password == result.Password);
            Assert.That(user.Age == result.Age);
            Assert.That(user.PermissionLevel == result.PermissionLevel);

            _mockRepository.Verify(repo => repo.UpdateUser(updatedUser), Times.Once);
        }

        [Test]
        public void RemoveUser_CallsRepositoryRemoveUser()
        {
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL, PermissionLevel = PermissionLevel.USER };
            _movieService.RemoveUser(user);
            _mockRepository.Verify(repo => repo.RemoveUser(user), Times.Once);
        }

        #endregion

        #region Tickets and Cart

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

            var result = _movieService.RemoveTicketFromCart(1, 1);

            Assert.IsNotNull(result);
            Assert.That(0 == result.Tickets.Count);
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
            Assert.That(1 == cart.Tickets.Count);
            Assert.That(2 == cart.Tickets.First().Quantity);
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

        [Test]
        public void GetTickets_ReturnsCorrectTickets()
        {
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailible = 20 }
            };
            _mockRepository.Setup(repo => repo.GetMovies()).Returns(new List<Movie> { new Movie { Id = 1, Tickets = tickets } });
            var result = _movieService.GetTickets(1);
            Assert.That(tickets == result);
        }

        [Test]
        public void GetCart_ReturnsCorrectCart()
        {
            var cart = new Cart { Id = 1, Tickets = new List<CartItem>() };
            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart> { cart });
            var result = _movieService.GetCart(1);
            Assert.That(cart == result);
        }

        [Test]
        public void GetCart_CreatesNewCartIfNotFound()
        {
            _mockRepository.Setup(repo => repo.GetCarts()).Returns(new List<Cart>());
            var result = _movieService.GetCart(1);
            Assert.IsNotNull(result);
            Assert.That(0 == result.Id);
            _mockRepository.Verify(repo => repo.AddCart(It.IsAny<Cart>()), Times.Once);
        }

        [Test]
        public void EditTickets_CallsRepositoryEditTickets()
        {
            DateTime now = DateTime.UtcNow;
            var ticket = new Ticket { Id = 1, MovieId = 1, Showtime = now, Price = 2.50, NumAvailible = 20 };
            var updatedTicket = new UpdatedTicket { Id = 1, MovieId = 1, Price = 3.50, NumAvailible = 15 };
            var expectedTicket = new Ticket { Id = 1, MovieId = 1, Showtime = now, Price = 3.50, NumAvailible = 15 };
            _mockRepository.Setup(repo => repo.EditTickets(1, updatedTicket)).Returns(expectedTicket);
            var result = _movieService.EditTickets(1, updatedTicket);

            Assert.That(updatedTicket.Price == result.Price);
            Assert.That(updatedTicket.NumAvailible == result.NumAvailible);
            Assert.That(ticket.Showtime == result.Showtime);

            _mockRepository.Verify(repo => repo.EditTickets(1, updatedTicket), Times.Once);
        }

        [Test]
        public void AddTicketToMovie_CallsRepositoryAddMovie()
        {
            var movie = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            var ticket = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.UtcNow, Price = 2.50, NumAvailible = 20 };
            _movieService.AddTicketsToMovie(ticket);
            _mockRepository.Verify(repo => repo.AddTicketsToMovie(ticket), Times.Once);
        }

        [Test]
        public void AddTicket_CallsRepositoryAddTicket()
        {
            var ticket = new Ticket { Id = 1, MovieId = 1, Showtime = DateTime.Now, Price = 10.0, NumAvailible = 100 };
            _mockRepository.Setup(repo => repo.AddTicket(ticket)).Returns(true);
            _movieService.AddTicket(ticket);
            _mockRepository.Verify(repo => repo.AddTicket(ticket), Times.Once);
        }

        [Test]
        public void RemoveTicketsFromMovie_CallsRepositoryRemoveTicketsFromMovie()
        {
            var movieId = 1;
            var numTickets = 10;
            _mockRepository.Setup(repo => repo.RemoveTicketsFromMovie(movieId, numTickets)).Returns(true);
            var result = _movieService.RemoveTicketsFromMovie(movieId, numTickets);
            Assert.IsTrue(result);
            _mockRepository.Verify(repo => repo.RemoveTicketsFromMovie(movieId, numTickets), Times.Once);
        }

        #endregion

        #region Payment

        [Test]
        public void ProcessPayment_ValidDetails_ProcessesPayment()
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
            _mockRepository.Setup(repo => repo.GetTickets()).Returns(new List<Ticket> { cart.Tickets.First().Ticket });

            Assert.DoesNotThrow(() => _movieService.ProcessPayment(1, "123 Main St", "Columbia", "SC", "29201", "1234567812345678", "12/2025", "John Doe", "123"));
            Assert.That(0 == cart.Tickets.Count);
        }

        [Test]
        public void ProcessPayment_InvalidCardNumber_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _movieService.ProcessPayment(1, "123 Main St", "Columbia", "SC", "29201", "123", "12/2025", "John Doe", "123"));
            Assert.That("Card number is invalid. Payment could not be processed." == ex.Message);
        }

        [Test]
        public void ProcessPayment_ExpiredCard_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _movieService.ProcessPayment(1, "123 Main St", "Columbia", "SC", "29201", "1234567812345678", "12/2020", "John Doe", "123"));
            Assert.That("Card is expired. Payment could not be processed." == ex.Message);
        }

        public void ProcessPayment_InvalidStreetAddress_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _movieService.ProcessPayment(1, "123 Main", "Columbia", "SC", "29201", "1234567812345678", "12/2020", "John Doe", "123"));
            Assert.That("Invalid street number. Payment could not be processed." == ex.Message);
        }

        public void ProcessPayment_InvalidCity_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _movieService.ProcessPayment(1, "123 Main St", "Columb1a", "SC", "29201", "1234567812345678", "12/2020", "John Doe", "123"));
            Assert.That("Invalid city. Payment could not be processed." == ex.Message);
        }

        public void ProcessPayment_InvalidState_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _movieService.ProcessPayment(1, "123 Main St", "Columbia", "S", "29201", "1234567812345678", "12/2020", "John Doe", "123"));
            Assert.That("Invalid state abbreviation. Payment could not be processed." == ex.Message);
        }

        public void ProcessPayment_InvalidZipCode_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _movieService.ProcessPayment(1, "123 Main St", "Columbia", "S", "2920", "1234567812345678", "12/2020", "John Doe", "123"));
            Assert.That("Invalid zip code. Payment could not be processed." == ex.Message);
        }

        #endregion

        #region Reviews

        [Test]
        public void EditReview_ReturnsEditedReview()
        {
            DateTime now = DateTime.UtcNow;

            var movie = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            var review = new Review { Id = 1, MovieId = 1, UserId = 1, PostDate = now, Comment = "Comment 1", Rating = 5, LikeCount = 1 };
            var updatedReview = new UpdatedReview { Id = 1, PostDate = now, Comment = "Comment 2", Rating = 4, Anonymous = false };
            var expectedReview = new Review { Id = 1, MovieId = 1, UserId = 1, PostDate = now, Comment = "Comment 2", Rating = 4, LikeCount = 2 };
            _mockRepository.Setup(repo => repo.EditReview(updatedReview)).Returns(expectedReview);
            var result = _movieService.EditReview(updatedReview);

            Assert.That(updatedReview.PostDate == result.PostDate);
            Assert.That(updatedReview.Comment == result.Comment);
            Assert.That(updatedReview.Rating == result.Rating);
            Assert.That(updatedReview.Anonymous == result.Anonymous);

            _mockRepository.Verify(repo => repo.EditReview(updatedReview), Times.Once);
        }

        [Test]
        public void RemoveReview_CallsRepositoryRemoveReview()
        {
            var movie = new Movie { Id = 1, Name = "Movie 1", Description = "Description 1" };
            var user = new User { Id = 1, Username = "Username 1", Password = "Password 1", Name = "Name 1", Gender = Gender.MALE, Age = 40, Email = "Email 1", PhoneNumber = "PhoneNumber 1", Preference = Preference.EMAIL, PermissionLevel = PermissionLevel.USER };
            var review = new Review { Id = 1, MovieId = 1, UserId = 1, PostDate = DateTime.UtcNow, Comment = "Comment 1", Rating = 5, LikeCount = 1 };
            _movieService.RemoveReview(review);
            _mockRepository.Verify(repo => repo.RemoveReview(review), Times.Once);
        }

        [Test]
        public void AddComment_CallsRepositoryAddComment()
        {
            var comment = new Comment { Id = 1, ReviewId = 1, Message = "Great movie!", Username = "user1" };
            _mockRepository.Setup(repo => repo.AddComment(comment)).Returns(true);
            var result = _movieService.AddComment(comment);
            Assert.That(result);
            _mockRepository.Verify(repo => repo.AddComment(comment), Times.Once);
        }

        [Test]
        public void GetComments_ReturnsListOfComments()
        {
            var reviewId = 1;
            var comments = new List<Comment> { new Comment { Id = 1, ReviewId = reviewId, Message = "Great movie!", Username = "user1" } };
            _mockRepository.Setup(repo => repo.GetComments(reviewId)).Returns(comments);
            var result = _movieService.GetComments(reviewId);
            Assert.That(comments == result);
            _mockRepository.Verify(repo => repo.GetComments(reviewId), Times.Once);
        }

        [Test]
        public void Liked_ReturnsTrueIfLiked()
        {
            var userId = 1;
            var reviewId = 1;
            _mockRepository.Setup(repo => repo.Liked(userId, reviewId)).Returns(true);
            var result = _movieService.Liked(userId, reviewId);
            Assert.IsTrue(result);
            _mockRepository.Verify(repo => repo.Liked(userId, reviewId), Times.Once);
        }

        [Test]
        public void AddLike_CallsRepositoryAddLike()
        {
            var userId = 1;
            var reviewId = 1;
            _mockRepository.Setup(repo => repo.AddLike(userId, reviewId)).Returns(true);
            var result = _movieService.AddLike(userId, reviewId);
            Assert.That(result);
            _mockRepository.Verify(repo => repo.AddLike(userId, reviewId), Times.Once);
        }

        [Test]
        public void AddReview_CallsRepositoryAddReview()
        {
            var review = new Review { Id = 1, MovieId = 1, UserId = 1, PostDate = DateTime.UtcNow, Comment = "Comment 1", Rating = 5, LikeCount = 1 };
            _mockRepository.Setup(repo => repo.AddReview(review)).Returns(1);
            var result = _movieService.AddReview(review);
            Assert.That(1 == result);
            _mockRepository.Verify(repo => repo.AddReview(review), Times.Once);
        }

        [Test]
        public void RemoveLike_CallsRepositoryRemoveLike()
        {
            var userId = 1;
            var reviewId = 1;
            _mockRepository.Setup(repo => repo.RemoveLike(userId, reviewId)).Returns(true);
            var result = _movieService.RemoveLike(userId, reviewId);
            Assert.IsTrue(result);
            _mockRepository.Verify(repo => repo.RemoveLike(userId, reviewId), Times.Once);
        }

        #endregion

    }
}
