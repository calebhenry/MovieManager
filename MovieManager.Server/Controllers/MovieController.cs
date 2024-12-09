using Microsoft.AspNetCore.Mvc;
using MovieManager.Server.Models;
using MovieManager.Server.Services;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace MovieManager.Server.Controllers
{
    /// <summary>
    /// Controller for handing all API endpoints. 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private IMovieService movieService;

        /// <summary>
        /// Constructor to initialize the controller with the movie service
        /// </summary>
        /// <param name="movieService"></param>
        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        /// <summary>
        /// Gets a list of all movies
        /// </summary>
        [HttpGet("getmovies", Name = "GetMovies")]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            return Ok(movieService.GetMovies().ToArray()); // Returns the array of movies
        }

        /// <summary>
        /// Adds a movie
        /// </summary>
        /// <param name="movie"></param>
        [HttpPost("addmovie", Name = "AddMovie")]
        public ActionResult AddMovie(Movie movie)
        {
            try
            {
                movieService.AddMovie(movie);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Return bad request if the messages fail
            }
        }

        /// <summary>
        /// Remove a movie
        /// </summary>
        /// <param name="movie"></param>
        [HttpDelete("removemovie", Name = "RemoveMovie")]
        public ActionResult RemoveMovie(Movie movie)
        {
            bool result = movieService.RemoveMovie(movie);
            if (result)
                return Ok();
            else
                return NotFound();
        }

        /// <summary>
        /// Get a movie by movie ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("getmovie/{id}", Name = "GetMovie")]
        public ActionResult<Movie> GetMovie(int id)
        {
            var movie = movieService.GetMovieById(id);
            if (movie != null)
            {
                return Ok(movie);
            }
            return NotFound();
        }

        /// <summary>
        /// Get a all liked reviews
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reviewId"></param>
        [HttpGet("liked/{userId}:{reviewId}", Name = "Liked")]
        public ActionResult<bool> Liked(int userId, int reviewId)
        {
            return Ok(movieService.Liked(userId, reviewId));
        }

        /// <summary>
        /// Like a review
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reviewId"></param>
        [HttpPost("like/{userId}:{reviewId}", Name = "Like")]
        public ActionResult Like(int userId, int reviewId)
        {
            if (movieService.AddLike(userId, reviewId))
            {
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Remove a like from a review
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reviewId"></param>
        [HttpDelete("removelike/{userId}:{reviewId}", Name = "RemoveLike")]
        public ActionResult RemoveLike(int userId, int reviewId)
        {
            if (movieService.RemoveLike(userId, reviewId))
            {
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Add a review
        /// </summary>
        /// <param name="review"></param>
        [HttpPost("addreview", Name = "AddReview")]
        public ActionResult<int> AddReview(Review review)
        {
            int result = movieService.AddReview(review);
            if (result != 0)
            {
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        ///  Adds tickets to a cartt
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ticketId"></param>
        /// <param name="quantity"></param>
        [HttpPost("addtickettocart", Name = "AddTicketToCart")]
        public ActionResult AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            if (movieService.AddTicketToCart(cartId, ticketId, quantity))
            {
                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Remove tickets from a cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpPut("removeticketfromcart")]
        public ActionResult<Cart> RemoveTicketFromCart(int cartId, int ticketId)
        { 
            var cart = movieService.RemoveTicketFromCart(ticketId, cartId);
            if (cart != null)
            {
                return Ok(cart);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets user (login)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [HttpGet("getuser", Name = "GetUser")]
        public ActionResult<User> GetUser(string username, string password)
        {
            var user = movieService.GetUser(username, password);
            return user == null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Adds a user (signup)
        /// </summary>
        /// <param name="user"></param>
        [HttpPost("adduser", Name = "AddUser")]
        public ActionResult<User> AddUser(User user)
        {
            return Ok(movieService.AddUser(user));
        }

        /// <summary>
        /// Update a user information
        /// </summary>
        /// <param name="updatedUser"></param>
        [HttpPut("updateuser", Name = "UpdateUser")]
        public ActionResult<User> UpdateUser(UpdatedUser updatedUser)
        { 
            return Ok(movieService.UpdateUser(updatedUser));
        }

        /// <summary>
        /// Remove a user
        /// </summary>
        /// <param name="user"></param>
        [HttpPost("removeuser", Name = "RemoveUser")]
        public ActionResult RemoveUser(User user)
        {
            movieService.RemoveUser(user);
            return Ok();
        }

        /// <summary>
        /// Gets card information for payment
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="streetAddress"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <param name="cardNumber"></param>
        /// <param name="exp"></param>
        /// <param name="cardholderName"></param>
        /// <param name="cvc"></param>
        [HttpPost("processpayment", Name = "ProcessPayment")]
        public ActionResult ProcessPayment(int cartId, string streetAddress, string city, string state, string zipCode, string cardNumber, string exp, string cardholderName, string cvc)
        {
            try
            {
                movieService.ProcessPayment(cartId, streetAddress, city, state, zipCode, cardNumber, exp, cardholderName, cvc);
                return Ok();
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all tickets of all movies
        /// </summary>
        [HttpGet("getalltickets", Name = "GetAllTickets")]
        public ActionResult<IEnumerable<Ticket>> GetAllTickets()
        {
            return Ok(movieService.GetAllTickets().ToArray());
        }

        /// <summary>
        /// Gets a list of all tickets of a movie from its movie ID
        /// </summary>
        /// <param name="movieId"></param>
        [HttpGet("gettickets", Name = "GetTickets")]
        public ActionResult<IEnumerable<Ticket>> GetTickets(int movieId)
        {
            return Ok(movieService.GetTickets(movieId).ToArray());
        }

        /// <summary>
        /// Get Cart
        /// </summary>
        /// <param name="cartId"></param>
        [HttpGet("getcart", Name = "GetCart")]

        public ActionResult<Cart> GetCart(int? cartId)
        {
            var cart = movieService.GetCart(cartId);
            return Ok(cart);
        }

        /// <summary>
        /// Remove ticket from one movie
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpDelete("removeticketfrommovie", Name="RemoveTicketFromMovie")]
        public ActionResult RemoveTicketFromMovie(Ticket ticket)
        { 
            try
            {
               movieService.RemoveTicket(ticket);
               return Ok();
            } catch (ArgumentException ex) {
               return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edit review
        /// </summary>
        /// <param name="updatedReview"></param>
        [HttpPut("editreview", Name = "EditReview")]
        public ActionResult<Review> EditReview(UpdatedReview updatedReview)
        {
            return Ok(movieService.EditReview(updatedReview));
        }

        /// <summary>
        /// Edit tickets (actually adding showtime and numbeer of tickets avilable)
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="updatedTicket"></param>
        [HttpPut("edittickets", Name = "EditTickets")]
        public ActionResult<Movie> EditTickets(int movieId, UpdatedTicket updatedTicket)
        {
            return Ok(movieService.EditTickets(movieId, updatedTicket));
        }

        /// <summary>
        /// Edit a movie 
        /// </summary>
        /// <param name="updatedMovie"></param>
        /// <returns></returns>
        [HttpPut("editmovie", Name = "EditMovie")]
        public ActionResult<Movie> EditMovie(UpdatedMovie updatedMovie)
        {
            return Ok(movieService.EditMovie(updatedMovie));
        }

        /// <summary>
        /// Gets all review of a movie
        /// </summary>
        /// <param name="movieId"></param>
        [HttpGet("getreviews", Name = "GetReviews")]
        public ActionResult<List<ReviewDTO>> GetReviews(int movieId)
        {
            var reviews = movieService.GetReviews(movieId);
            // we don't want to give the client the whole user object, but we do want them
            // to have a username for the review if not anonymous
            return Ok((from i in reviews select new ReviewDTO(i, i.User.Username)).ToList());
        }

        /// <summary>
        /// Add a comment to a review
        /// </summary>
        /// <param name="comment"></param>
        [HttpPost("addcomment", Name = "AddComment")]
        public ActionResult AddComment(Comment comment)
        {
            if (movieService.AddComment(comment))
            {
                return Ok();
            } else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all comments of a review
        /// </summary>
        /// <param name="reviewId"></param>
        [HttpGet("getcomments", Name = "GetComments")]
        public ActionResult<List<Comment>> GetComments(int reviewId)
        {
                return Ok(movieService.GetComments(reviewId));
        }

        /// <summary>
        /// Adds ticket to one movie
        /// </summary>
        /// <param name="ticket"></param>
        [HttpPost("addticketstomovie", Name = "AddTicketsToMovie")]
        public ActionResult<Movie> AddTicketsToMovie(Ticket ticket)
        {
            try
            {
                movieService.AddTicketsToMovie(ticket);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove review
        /// </summary>
        /// <param name="review"></param>
        [HttpDelete("removereview", Name = "RemoveReview")]
        public ActionResult RemoveReview(Review review)
        {
            bool result = movieService.RemoveReview(review);
            if (result)
                return Ok();
            else
                return NotFound();
        }
        
        /// <summary>
        /// Removes tickets from a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="numTickets"></param>
        /// <returns></returns>
        [HttpDelete("removeticketsfrommovie", Name = "RemoveTicketsFromMovie")]
        public ActionResult RemoveTicketsFromMovie(int movieId, int numTickets)
        {
            bool result  = movieService.RemoveTicketsFromMovie(movieId, numTickets);
            if (result){
                return Ok();
            } else return NotFound();
        }
    }
}