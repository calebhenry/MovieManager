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

        [HttpPost("addreview", Name = "AddReview")]
        public ActionResult AddReview(Review review)
        {
            if (movieService.AddReview(review))
            {
                return Ok();
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

        //currentUserId should come from the logged in user, this is to amke sure the user can only edit their own comments
        [HttpPut("editreview", Name = "EditReview")]
        public ActionResult<Review> EditReview(int currentUserId, UpdatedReview updatedReview)
        {
            try
            {
                movieService.EditReview(currentUserId, updatedReview);
                return Ok();
            } catch (ArgumentException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edittickets", Name = "EditTickets")]
        public ActionResult<Movie> EditTickets(int movieId, UpdatedTicket updatedTicket)
        {
            return Ok(movieService.EditTickets(movieId, updatedTicket));
        }

        [HttpGet("getreviews", Name = "GetReviews")]
        public ActionResult<List<Review>> GetReviews(int movieId)
        {
            var reviews = movieService.GetReviews(movieId);
            return Ok(reviews);
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