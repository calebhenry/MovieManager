using MovieManager.Server.Models;
using System;

namespace MovieManager.Server.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private List<Movie> movies;
        private List<Cart> carts;
        private List<Ticket> tickets;

        public MovieRepository()
        {
            movies = new List<Movie>();
            carts = new List<Cart>();
            tickets = new List<Ticket>();
            Movie movie1 = new Movie()
            {
                Id = 1,
                Name = "Grand Budapest Hotel",
                Description = "It's about a big hotel.",
                Tickets = new()
            };

            List<Ticket> ticket1 = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = 1,
                    MovieId = 1,
                    Showtime = DateTime.UtcNow,
                    Price = 2.50,
                    NumAvailible = 20,
                },
                new Ticket()
                {
                    Id = 2,
                    MovieId = 1,
                    Showtime = DateTime.UtcNow.AddHours(-2),
                    Price = 2.0,
                    NumAvailible = 25,
                }
            };

            movie1.Tickets = ticket1;

            Movie movie2 = new Movie()
            {
                Id = 2,
                Name = "Mini Budapest Hotel",
                Description = "It's about a very small hotel.",
                Tickets = new()
            };

            List<Ticket> ticket2 = new List<Ticket>()
            {
                new Ticket()
                {
                    Id = 0,
                    MovieId = 2,
                    Showtime = DateTime.UtcNow,
                    Price = 2.50,
                    NumAvailible = 20,
                },
                new Ticket()
                {
                    Id = 3,
                    MovieId = 2,
                    Showtime = DateTime.UtcNow.AddHours(-2),
                    Price = 2.0,
                    NumAvailible = 25,
                }
            };

            movie2.Tickets = ticket2;
            tickets.Add(ticket2[0]);
            tickets.Add(ticket2[1]);
            tickets.Add(ticket1[0]);
            tickets.Add(ticket1[1]);
            movies.Add(movie1);
            movies.Add(movie2);
            Dictionary<Ticket, int> tickets1 = new Dictionary<Ticket, int>();
            Dictionary<Ticket, int> tickets2 = new Dictionary<Ticket, int>();
            tickets1[ticket1[0]] = 3;
            tickets1[ticket1[1]] = 2;
            tickets2[ticket2[0]] = 1;
            tickets2[ticket2[1]] = 5;
            Cart cart1 = new Cart();
            cart1.Id = 0;
            cart1.Tickets = tickets1;
            Cart cart2 = new Cart();
            cart2.Id = 0;
            cart2.Tickets = tickets2;
            carts.Add(cart1);
            carts.Add(cart2);
        }

        public List<Movie> GetMovies()
        {
            return movies;
        }

        public void AddMovie(Movie movie)
        {
            movies.Add(movie);
        }

        public void RemoveMovie(Movie movie)
        {
            movies.Remove(movie);
        }

        public List<Ticket> GetTickets()
        {
            return tickets;
        }

        public void AddCart(Cart cart)
        {
            carts.Add(cart);
        }

        public void AddTicket(Ticket ticket)
        {
            tickets.Add(ticket);
        }

        public void RemoveTicket(Ticket ticket)
        {
            tickets.Remove(ticket);
        }

        public List<Cart> GetCarts()
        {
            return carts;
        }
    }
}
