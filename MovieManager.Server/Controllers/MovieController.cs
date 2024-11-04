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
                cart.Tickets = new List<Ticket>();
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
    }
}
