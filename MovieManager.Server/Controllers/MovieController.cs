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
        public IEnumerable<Movie> GetMovies()
        {
            return movieService.GetMovies().ToArray();
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
        public Cart RemoveTicketFromCart(int ticketId, int cartId)
        { 
            var cart = movieService.RemoveTicket(ticketId, cartId);
            if (cart != null)
            {
                return cart;
            }
            else
            {
                // todo return error code instead
                cart = new Cart();
                cart.Id = ticketId;
                cart.Tickets = new List<CartItem>();
                movieService.AddCart(cart);
                return cart;
            }
        }

        [HttpGet("getuser", Name = "GetUser")]
        public User? GetUser(string username, string password)
        {
            return movieService.GetUser(username, password);
        }

        [HttpPost("adduser", Name = "AddUser")]
        public HttpStatusCode AddUser(User user)
        {
            movieService.AddUser(user);
            return HttpStatusCode.OK;
        }

        [HttpPut("updateuser", Name = "UpdateUser")]
        public User UpdateUser(UpdatedUser updatedUser)
        { 
            return movieService.UpdateUser(updatedUser);
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
        public IEnumerable<Ticket> GetTickets(int movieId)
        {
            return movieService.GetTickets(movieId).ToArray();
        }
        [HttpGet("getcart", Name = "GetCart")]
        public Cart GetCart(int cartId)
        {
            return movieService.GetCart(cartId);
        }
    }
}