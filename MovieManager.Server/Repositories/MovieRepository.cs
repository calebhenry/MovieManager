using Microsoft.EntityFrameworkCore;
using MovieManager.Server.Models;
using System;

namespace MovieManager.Server.Repositories
{
    public class MovieRepository : IMovieRepository
    {

        public class MovieContext : DbContext
        {

            public DbSet<Movie> Movies { get; set; }
            public DbSet<Cart> Carts { get; set; }
            public DbSet<Ticket> Tickets { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Review> Reviews { get; set; }
            private DbSet<CartItem> CartItems { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseSqlServer(System.Environment.GetEnvironmentVariable("movieDb"));
            }

        }

        public List<Movie> GetMovies()
        {

            var db = new MovieContext();
            return db.Movies.ToList();
        }

        public Movie? GetMovieById(int id)
        {
            var db = new MovieContext();
            return (from i in db.Movies where i.Id == id select i).ToList().FirstOrDefault();
        }

        public void AddMovie(Movie movie)
        {
            movie.Id = 0;
            var db = new MovieContext();
            movie.Id = 0;
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        public bool RemoveMovie(Movie movie)
        {
            var db = new MovieContext();
            var movieRem = (from i in db.Movies where i.Id == movie.Id select i).ToList().FirstOrDefault();
            if (movieRem == null)
            {
                return false;
            }
            // TODO: remove everything that references that movie, or references a ticket for that movie ?
            db.Movies.Remove(movieRem);
            db.SaveChanges();
            return true;

        }

        public List<Ticket> GetTickets()
        {
            var db = new MovieContext();
            return db.Tickets.ToList();
        }

        public void AddCart(Cart cart)
        {
            cart.Id = 0;
            var db = new MovieContext();
            db.Carts.Add(cart);
            db.SaveChanges();
        }

        public bool AddTicket(Ticket ticket)
        {
            ticket.Id = 0;
            var db = new MovieContext();
            var movie = (from i in db.Movies where i.Id == ticket.MovieId select i).ToList().FirstOrDefault();
            if (movie == null) {
                return false;
            }
            db.Update(movie);
            movie.Tickets.Add(ticket);
            db.Tickets.Add(ticket);
            db.SaveChanges();
            return true;
        }

        public void RemoveTicket(Ticket ticket)
        {
            var db = new MovieContext();
            var tick = (from i in db.Tickets where i.Id == ticket.Id select i).FirstOrDefault();
            if (tick == null) { return; }
            db.Tickets.Remove(tick);
            db.SaveChanges();
            // TODO: remove all other references ?
        }

        public List<Cart> GetCarts()
        {
            var db = new MovieContext();
            return db.Carts.ToList();
        }

        public List<User> GetUsers()
        {
            var db = new MovieContext();
            return db.Users.ToList();
        }

        public void AddUser(User user)
        {
            user.Id = 0;
            var db = new MovieContext();
            db.Users.Add(user);
            db.SaveChanges();
        }

        public User? GetUser(string username, string password)
        {
            var db = new MovieContext();
            return (from i in db.Users where i.Username == username &&
                    i.Password == password select i).FirstOrDefault();
        }

        public User? UpdateUser(UpdatedUser updatedUser)
        {
            var db = new MovieContext();
            var user = (from i in db.Users where i.Id == updatedUser.Id select i).ToList().FirstOrDefault();
            if (user == null) { 
                return null; 
            }
            db.Update(user);
            if (!string.IsNullOrEmpty(updatedUser.Name))
            {
                user.Name = updatedUser.Name;
            }
            if (!string.IsNullOrEmpty(updatedUser.Email))
            {
                user.Email = updatedUser.Email;
            }
            if (!string.IsNullOrEmpty(updatedUser.PhoneNumber))
            {
                user.PhoneNumber = updatedUser.PhoneNumber;
            }
            if (updatedUser.Preference != null)
            {
                user.Preference = (Preference) updatedUser.Preference;
            }
            db.SaveChanges();
            return user;
        }

        public void RemoveUser(User user)
        {
            var db = new MovieContext();
            var usrRemove = (from i in db.Users where i.Id == user.Id select i).ToList().FirstOrDefault(); 
            if (usrRemove == null) { return; }
            db.Users.Remove(usrRemove);
            db.SaveChanges();
            // TODO: Remove all user data ?
        }

        public Ticket EditTickets(int movieId, UpdatedTicket updatedTicket)
        {
            var db = new MovieContext();
            var ticket = (from i in db.Tickets where i.Id == updatedTicket.Id select i).ToList().FirstOrDefault();
            if (ticket == null) { 
                return null; 
            }
            db.Update(ticket);
            
            ticket.Price = updatedTicket.Price ?? ticket.Price;

            ticket.NumAvailible = updatedTicket.NumAvailible ?? ticket.NumAvailible;

            db.SaveChanges();
            return ticket;
        }
    }
}
