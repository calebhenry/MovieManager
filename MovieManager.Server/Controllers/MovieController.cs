using Microsoft.AspNetCore.Mvc;
using MovieManager.Server.Models;
using MovieManager.Server.Services;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace MovieManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {

        private IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet("getmovies", Name = "GetMovies")]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            return Ok(movieService.GetMovies().ToArray()); // Returns the array of movies
        }

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

        [HttpDelete("removemovie", Name = "RemoveMovie")]
        public ActionResult RemoveMovie(Movie movie)
        {
            bool result = movieService.RemoveMovie(movie);
            if (result)
                return Ok();
            else
                return NotFound();
        }

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

        [HttpGet("liked/{userId}:{reviewId}", Name = "Liked")]
        public ActionResult<bool> Liked(int userId, int reviewId)
        {
            return Ok(movieService.Liked(userId, reviewId));
        }

        [HttpPost("like/{userId}:{reviewId}", Name = "Like")]
        public ActionResult Like(int userId, int reviewId)
        {
            if (movieService.AddLike(userId, reviewId))
            {
                return Ok();
            }
            return NotFound();
        }

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

        [HttpPost("addtickettocart", Name = "AddTicketToCart")]
        public ActionResult AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            if (movieService.AddTicketToCart(cartId, ticketId, quantity))
            {
                return Ok();
            }
            return NotFound();
        }

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

        [HttpGet("getuser", Name = "GetUser")]
        public ActionResult<User> GetUser(string username, string password)
        {
            var user = movieService.GetUser(username, password);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("adduser", Name = "AddUser")]
        public ActionResult<User> AddUser(User user)
        {
            return Ok(movieService.AddUser(user));
        }

        [HttpPut("updateuser", Name = "UpdateUser")]
        public ActionResult<User> UpdateUser(UpdatedUser updatedUser)
        { 
            return Ok(movieService.UpdateUser(updatedUser));
        }

        [HttpPost("removeuser", Name = "RemoveUser")]
        public ActionResult RemoveUser(User user)
        {
            movieService.RemoveUser(user);
            return Ok();
        }

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
        [HttpGet("getalltickets", Name = "GetAllTickets")]
        public ActionResult<IEnumerable<Ticket>> GetAllTickets()
        {
            return Ok(movieService.GetAllTickets().ToArray());
        }

        [HttpGet("gettickets", Name = "GetTickets")]
        public ActionResult<IEnumerable<Ticket>> GetTickets(int movieId)
        {
            return Ok(movieService.GetTickets(movieId).ToArray());
        }

        [HttpGet("getcart", Name = "GetCart")]

        public ActionResult<Cart> GetCart(int? cartId)
        {
            var cart = movieService.GetCart(cartId);
            return Ok(cart);
        }

        [HttpDelete("removeticketfrommovie", Name="RemoveTicketFromMovie")]
        public ActionResult RemoveTicketFromMovie(int movieId, int NumAvailible)
        { 
             try
            {
                movieService.RemoveTicketFromMovie(movieId, NumAvailible);
                return Ok();
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("editreview", Name = "EditReview")]
        public ActionResult<Review> EditReview(UpdatedReview updatedReview)
        {
            return Ok(movieService.EditReview(updatedReview));
        }

        [HttpPut("edittickets", Name = "EditTickets")]
        public ActionResult<Movie> EditTickets(UpdatedTicket updatedTicket)
        {
            return Ok(movieService.EditTickets(updatedTicket));
        }

        [HttpPut("editmovie", Name = "EditMovie")]
        public ActionResult<Movie> EditMovie(UpdatedMovie updatedMovie)
        {
            return Ok(movieService.EditMovie(updatedMovie));
        }

        [HttpGet("getreviews", Name = "GetReviews")]
        public ActionResult<List<ReviewDTO>> GetReviews(int movieId)
        {
            var reviews = movieService.GetReviews(movieId);
            // we don't want to give the client the whole user object, but we do want them
            // to have a username for the review if not anonymous
            return Ok((from i in reviews select new ReviewDTO(i, i.User.Username)).ToList());
        }

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
    }
}