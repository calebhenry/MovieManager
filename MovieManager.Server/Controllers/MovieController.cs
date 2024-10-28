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

        private MovieService movieService;

        public MovieController(MovieService movieService)
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
                cart = new Cart();
                cart.Id = ticketId;
                cart.Tickets = new List<Ticket>();
                movieService.AddCart(cart);
                return cart;
            }
        }
    }
}
