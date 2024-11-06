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
            return Ok(movieService.GetMovies().ToArray());
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("removemovie", Name = "RemoveMovie")]
        public ActionResult RemoveMovie(Movie movie)
        {
            movieService.RemoveMovie(movie);
            return Ok();
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
        public ActionResult<Cart> RemoveTicketFromCart(int ticketId, int cartId)
        { 
            var cart = movieService.RemoveTicket(ticketId, cartId);
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
        public ActionResult ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc)
        {
            try
            {
                movieService.ProcessPayment(cartId, cardNumber, exp, cardholderName, cvc);
                return Ok();
            } catch (ArgumentException ex) {
                return BadRequest(ex);
            }
        }

        [HttpGet("gettickets", Name = "GetTickets")]
        public ActionResult<IEnumerable<Ticket>> GetTickets(int movieId)
        {
            return Ok(movieService.GetTickets(movieId).ToArray());
        }
        [HttpGet("getcart", Name = "GetCart")]
        public ActionResult<Cart> GetCart(int cartId)
        {
            var cart = movieService.GetCart(cartId);
            return cart == null ? NotFound() : Ok(cart);
        }
    }
}