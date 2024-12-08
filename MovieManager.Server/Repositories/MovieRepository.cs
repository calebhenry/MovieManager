using Microsoft.EntityFrameworkCore;
using MovieManager.Server.Models;
using System;

namespace MovieManager.Server.Repositories
{
    public class MovieRepository : IMovieRepository
    {

        private MovieContext _context;

        public MovieRepository(MovieContext context)
        {
            _context = context;
        }

        public List<Movie> GetMovies()
        {
            var movies = _context.Movies.Include(_ => _.Tickets).Include(_ => _.Reviews).ToList();
            return _context.Movies.ToList();
        }

        public Movie? GetMovieById(int id)
        {
            return _context.Movies
             .Include(m => m.Tickets)
             .Include(m => m.Reviews)
             .FirstOrDefault(m => m.Id == id);
        }

        public void AddMovie(Movie movie)
        {
            movie.Id = 0;
            movie.Tickets.ForEach(_ => _.Id = 0);
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public int AddReview(Review review)
        {
            review.Id = 0;
            var movie = (from i in _context.Movies where i.Id == review.MovieId select i).ToList().FirstOrDefault();
            if (movie == null)
                return 0;
            var user = (from i in _context.Users where i.Id == review.UserId select i).ToList().FirstOrDefault();
            if (user == null)
                return 0;
            _context.Update(user);
            _context.Update(movie);
            user.Reviews.Add(review);
            movie.Reviews.Add(review);
            _context.SaveChanges();
            var reviewNew = (from i in _context.Reviews
                          where i.UserId == user.Id &&
                          i.MovieId == movie.Id
                          select i).ToList().FirstOrDefault();
            if (reviewNew == null) return 0;
            return reviewNew.Id;
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
            return _context.Tickets.ToList();
        }

        public void AddCart(Cart cart)
        {
            cart.Id = 0;
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public bool AddTicket(Ticket ticket)
        {
            ticket.Id = 0;
            var movie = (from i in _context.Movies where i.Id == ticket.MovieId select i).ToList().FirstOrDefault();
            if (movie == null) {
                return false;
            }
            _context.Update(movie);
            movie.Tickets.Add(ticket);
            _context.SaveChanges();
            return true;
        }

        public void RemoveTicket(Ticket ticket)
        {
            var tick = (from i in _context.Tickets where i.Id == ticket.Id select i).FirstOrDefault();
            if (tick == null) { return; }
            _context.Tickets.Remove(tick);
            _context.SaveChanges();
            // TODO: remove all other references ?
        }

        public List<Cart> GetCarts()
        {
            return _context.Carts.Include(c => c.Tickets).ThenInclude(ci => ci.Ticket).ToList();
        }

        public void UpdateCart(Cart cart)
        {
            var existingCart = _context.Carts.Include(c => c.Tickets).FirstOrDefault(c => c.Id == cart.Id);

            if (existingCart != null)
            {
                foreach (var existingItem in existingCart.Tickets.ToList())
                {
                    if (!cart.Tickets.Any(t => t.Id == existingItem.Id))
                    {
                        _context.CartItems.Remove(existingItem);
                    }
                }

                _context.Entry(existingCart).CurrentValues.SetValues(cart);
                foreach (var item in cart.Tickets)
                {
                    var existingItem = existingCart.Tickets.FirstOrDefault(t => t.Id == item.Id);
                    if (existingItem != null)
                    {
                        _context.Entry(existingItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        existingCart.Tickets.Add(item);
                    }
                }

                _context.SaveChanges();
            }
        }
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUser(User user)
        {
            user.Id = 0;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User? GetUser(string username, string password)
        {
            return (from i in _context.Users.Include(_ => _.Reviews).ToList() where i.Username == username &&
                    i.Password == password select i).FirstOrDefault();
        }

        public User? UpdateUser(UpdatedUser updatedUser)
        {
            var user = (from i in _context.Users where i.Id == updatedUser.Id select i).ToList().FirstOrDefault();
            if (user == null) { 
                return null; 
            }
            _context.Update(user);
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
            _context.SaveChanges();
            return user;
        }

        public void RemoveUser(User user)
        {
            var userRemove = (from i in _context.Users where i.Id == user.Id select i).ToList().FirstOrDefault(); 
            if (userRemove == null) { return; }
            _context.Users.Remove(userRemove);
            _context.SaveChanges();
            // TODO: Remove all user data ?
        }

        public Review? EditReview(UpdatedReview updatedReview)
        {
            var review = (from i in _context.Reviews where i.MovieId == updatedReview.MovieId && 
                          i.UserId == updatedReview.UserId select i).ToList().FirstOrDefault();
            if (review == null) { 
                return null; 
            }
            _context.Update(review);
            review.PostDate = updatedReview.PostDate;
            if (!string.IsNullOrEmpty(updatedReview.Comment))
            {
                review.Comment = updatedReview.Comment;
            }
            if (updatedReview.Rating != null)
            {
                review.Rating = updatedReview.Rating ?? review.Rating;
            }
            review.Anonymous = updatedReview.Anonymous;
            _context.SaveChanges();
            return review;
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

        public Movie? EditMovie(UpdatedMovie updatedMovie)
        {
            if (updatedMovie.Id <= 0) 
                {
                    Console.WriteLine("Invalid Movie Id: " + updatedMovie.Id);
                    throw new ArgumentException("Invalid Id for the movie.");
                }

                using (var db = new MovieContext())
                {
                    var movie = db.Movies.FirstOrDefault(i => i.Id == updatedMovie.Id);
                    
                    if (movie == null)
                    {
                        Console.WriteLine($"Movie with Id {updatedMovie.Id} not found.");
                    }

                    Console.WriteLine($"Found movie with Id: {movie.Id}");

                    // Update movie fields only if they're not null
                    movie.Name = updatedMovie.Name ?? movie.Name;
                    movie.Description = updatedMovie.Description ?? movie.Description;
                    movie.Genre = updatedMovie.Genre;

                    db.SaveChanges();

                    // Return a response object with a message and the updated movie
                    return movie;
                }
        }

        public List<Review> GetReviews(int movieId)
        {
            return _context.Reviews.Where(r => r.MovieId == movieId).Include(_ => _.User).ToList();
        }

        public bool Liked(int userId, int reviewId)
        {
            return _context.Likes.Where(i => i.UserId == userId && i.ReviewId == reviewId).Any();
        }
        public bool AddLike(int userId, int reviewId)
        {
            var review = _context.Reviews.Where(i => i.Id == reviewId).ToList().FirstOrDefault();
            if (review == null) { return false; }
            var user = _context.Users.Where(i => i.Id == userId).ToList().FirstOrDefault();
            if (user == null) { return false; }
            _context.Update(review);
            Like like = new Like();
            like.UserId = user.Id;
            like.ReviewId = review.Id;
            _context.Likes.Add(like);
            review.LikeCount++;
            _context.SaveChanges();
            return true;
        }

        public void AddTicketsToMovie(Ticket ticket)
        {
            ticket.Id = 0;
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }
    }

    public class MovieContext : DbContext
    {

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(@"data source=maxbear123\SQLEXPRESS;initial catalog=Movies;trusted_connection=true;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Tickets)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId)
                .IsRequired();
            modelBuilder.Entity<Cart>()
                .HasMany(e => e.Tickets)
                .WithOne(e => e.Cart)
                .HasForeignKey(e => e.CartId)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Movie)
                .HasForeignKey(e => e.MovieId)
                .IsRequired();
            modelBuilder.Entity<CartItem>()
                .HasOne(e => e.Ticket);
        }
    }
}
