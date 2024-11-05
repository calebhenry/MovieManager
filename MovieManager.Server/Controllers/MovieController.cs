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

        [HttpPost("addmovie", Name = "AddMovie")]
        public HttpStatusCode AddMovie(Movie movie)
        {
            movieService.AddMovie(movie);
            return HttpStatusCode.OK;
        }

        [HttpPost("removemovie", Name = "RemoveMovie")]
        public HttpStatusCode RemoveMovie(Movie movie)
        {
            movieService.RemoveMovie(movie);
            return HttpStatusCode.OK;
        }

        [HttpPost("addtickettocart", Name = "AddTicketToCart")]
        public HttpStatusCode AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            if (movieService.AddTicketToCart(cartId, ticketId, quantity))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.NotFound;
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
        public HttpStatusCode AddUser(User user)
        {
            movieService.AddUser(user);
            return HttpStatusCode.OK;
        }

        [HttpPut("updateuser", Name = "UpdateUser")]
        public ActionResult<User> UpdateUser(UpdatedUser updatedUser)
        { 
            return Ok(movieService.UpdateUser(updatedUser));
        }

        [HttpPost("removeuser", Name = "RemoveUser")]
        public HttpStatusCode RemoveUser(User user)
        {
            movieService.RemoveUser(user);
            return HttpStatusCode.OK;
        }

        [HttpPost("processpayment", Name = "ProcessPayment")]
        public HttpStatusCode ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc)
        {
            try
            {
                movieService.ProcessPayment(cartId, cardNumber, exp, cardholderName, cvc);
                return HttpStatusCode.OK;
            } catch (ArgumentException ex) {
                return HttpStatusCode.BadRequest;
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