using Microsoft.EntityFrameworkCore;
using MovieManager.Server.Models;
using System;

namespace MovieManager.Server.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        /// <summary>
        /// Constructor to initialize the context
        /// </summary>
        private MovieContext _context;
        
        /// <summary>
        /// Adds a comment to a review if the review exists and message/username are not empty
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>bool</returns>
        public bool AddComment(Comment comment)
        {
            comment.Id = 0;
            if (comment.Message == "" || comment.Username == "")
            {
                return false;
            }
            var review = (from i in _context.Reviews where i.Id == comment.ReviewId select i).ToList().FirstOrDefault();
            if (review == null)
                return false;
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets all comments for a given review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>List of Comments</returns>
        public List<Comment> GetComments(int reviewId)
        {
            List<Comment> rtn = new List<Comment>();
            var review = (from i in _context.Reviews where i.Id == reviewId select i).ToList().FirstOrDefault();
            if (review == null)
                return rtn;
            var comments = (from i in _context.Comments where i.ReviewId == review.Id select i).ToList();
            if (comments == null)
                return rtn;
            foreach (var comment in comments)
            {
                rtn.Add(comment);
            }
            return rtn;
        }

        /// <summary>
        /// Removes a like from a review
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reviewId"></param>
        /// <returns>bool</returns>
        public bool RemoveLike(int userId, int reviewId)
        {
            var like = (from i in _context.Likes where i.UserId == userId &&
                        i.ReviewId == reviewId select i).ToList().FirstOrDefault();
            var review = (from i in _context.Reviews where i.Id == reviewId select i).ToList().FirstOrDefault();
            if (like != null && review != null)
            {
                _context.Update(review);
                review.LikeCount--;
                _context.Likes.Remove(like);
                _context.SaveChanges();
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Initializing context with and instance of MovieContext
        /// </summary>
        /// <param name="context"></param>
        public MovieRepository(MovieContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all movies with its tickets and reviews
        /// </summary>
        /// <returns>List of Movies</returns>
        public List<Movie> GetMovies()
        {
            var movies = _context.Movies.Include(_ => _.Tickets).Include(_ => _.Reviews).ToList();
            return _context.Movies.ToList();
        }

        /// <summary>
        /// Gets all ticks
        /// </summary>
        /// <returns>List of tickets</returns>
        public List<Ticket> GetAllTickets()
        {
            return _context.Tickets.ToList();
        }

        /// <summary>
        /// Gets a movie from its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Movie</returns>
        public Movie? GetMovieById(int id)
        {
            return _context.Movies
             .Include(m => m.Tickets)
             .Include(m => m.Reviews)
             .FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Adds a movie to the database
        /// </summary>
        /// <param name="movie"></param>
        public void AddMovie(Movie movie)
        {
            movie.Id = 0;
            movie.Tickets.ForEach(_ => _.Id = 0);
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        /// <summary>
        /// Adds a review to a movie if both the movie and user exists
        /// </summary>
        /// <param name="review"></param>
        /// <returns>Bool or review ID</returns>
        public int AddReview(Review review)
        {
            review.Id = 0;
            review.LikeCount = 0;
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

        /// <summary>
        /// Removes a movie with its reviews and tickets from database
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>bool</returns>
        public bool RemoveMovie(Movie movie)
        {
            var movieRem = (from i in _context.Movies where i.Id == movie.Id select i).ToList().FirstOrDefault();
            if (movieRem == null)
            {
                return false;
            }
            var tickets = (from i in _context.Tickets where i.MovieId == movie.Id select i).ToList();
            foreach (var ticket in tickets)
            {
                _context.Tickets.Remove(ticket);
            }
            var reviews = (from i in _context.Reviews where i.MovieId == movie.Id select i).ToList();
            foreach (var review in reviews)
            {
                _context.Reviews.Remove(review);
            }
            _context.Movies.Remove(movieRem);
            _context.SaveChanges();
            return true;

        }

        /// <summary>
        /// This function is used to get all tickets from movie ID
        /// </summary>
        /// <returns>List of tickets</returns>
        public List<Ticket> GetTickets()
        {
            return _context.Tickets.ToList();
        }

        /// <summary>
        /// Adds a cart to the database
        /// </summary>
        /// <param name="cart"></param>
        public void AddCart(Cart cart)
        {
            cart.Id = 0;
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        /// <summary>
        /// Adds ticket
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Removes Ticket
        /// </summary>
        /// <param name="ticket"></param>
        public void RemoveTicket(Ticket ticket)
        {
            var tick = (from i in _context.Tickets where i.Id == ticket.Id select i).FirstOrDefault();
            var movie = (from i in _context.Movies where i.Id == tick.MovieId select i).FirstOrDefault();
            if (tick == null || movie == null) { return; }
            _context.Tickets.Remove(tick);
            movie.Tickets.Remove(tick);
            _context.Update(movie);
            _context.SaveChanges();
        }


        /// <summary>
        /// Gets all carts with its tickets
        /// </summary>
        /// <returns>Cart</returns>
        public List<Cart> GetCarts()
        {
            return _context.Carts.Include(c => c.Tickets).ThenInclude(ci => ci.Ticket).ToList();
        }
        
        /// <summary>
        /// Updating a cart and its item
        /// </summary>
        /// <param name="cart"></param>
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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>User</returns>
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        /// <summary>
        /// Adds a user to the database (Sign Up)
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            user.Id = 0;
            user.PermissionLevel = PermissionLevel.USER;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets a user (Login)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>User</returns>
        public User? GetUser(string username, string password)
        {
            return (from i in _context.Users.Include(_ => _.Reviews).ToList() where i.Username == username &&
                    i.Password == password select i).FirstOrDefault();
        }

        /// <summary>
        /// Updates user into
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <returns>Updated User</returns>
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

        /// <summary>
        /// Remove User from database
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(User user)
        {
            var userRemove = (from i in _context.Users where i.Id == user.Id select i).ToList().FirstOrDefault(); 
            if (userRemove == null) { return; }
            _context.Users.Remove(userRemove);
            _context.SaveChanges();
            // TODO: Remove all user data ?
        }

        /// <summary>
        /// Edit a review
        /// </summary>
        /// <param name="updatedReview"></param>
        /// <returns>Updated Review</returns>
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

        /// <summary>
        /// Update Ticket, actually adding timeshow and number of avaiable tickets
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="updatedTicket"></param>
        /// <returns>Updated Ticket</returns>
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

        /// <summary>
        /// Edit a Movie's information
        /// </summary>
        /// <param name="updatedMovie"></param>
        /// <returns>Updated Movie</returns>
        /// <exception cref="ArgumentException"></exception>
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
                    movie.AgeRating = updatedMovie.AgeRating;

                    db.SaveChanges();

                    // Return a response object with a message and the updated movie
                    return movie;
                }
        }

        /// <summary>
        /// Get all reviews including its' users
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>Reviews</returns>
        public List<Review> GetReviews(int movieId)
        {
            return _context.Reviews.Where(r => r.MovieId == movieId).Include(_ => _.User).ToList();
        }

        /// <summary>
        /// Gets all likeed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reviewId"></param>
        /// <returns>Liked Reviews</returns>
        public bool Liked(int userId, int reviewId)
        {
            return _context.Likes.Where(i => i.UserId == userId && i.ReviewId == reviewId).Any();
        }

        /// <summary>
        /// Adds a like to a review
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="reviewId"></param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Adds a ticket to a movie using movie ID
        /// </summary>
        /// <param name="ticket"></param>
        public void AddTicketsToMovie(Ticket ticket)
        {
            ticket.Id = 0;
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes a reviews and its comments and likes
        /// </summary>
        /// <param name="review"></param>
        /// <returns>bool</returns>
        public bool RemoveReview(Review review)
        {
            var revRemove = (from i in _context.Reviews where i.Id == review.Id select i).ToList().FirstOrDefault();
            if (revRemove == null) { return false; }
            var comments = (from i in _context.Comments where i.ReviewId == revRemove.Id select i).ToList();
            var likes = (from i in _context.Likes where i.ReviewId == revRemove.Id select i).ToList();
            _context.Reviews.Remove(revRemove);
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    _context.Comments.Remove(comment);
                }
            }
            if (likes != null)
            {
                foreach (var like in likes)
                {
                    _context.Likes.Remove(like);
                } 
            }
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Removes a ticket from a movie (manager)
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="numTickets"></param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool RemoveTicketsFromMovie(int movieId, int numTickets)
        {
            if (numTickets <= 0)
            {
                return false;
                throw new ArgumentException("Number of tickets to remove must be greater than zero.");
            }
            var movie = _context.Movies.SingleOrDefault(m => m.Id == movieId);
            if (movie == null)
            {
                return false;
                throw new ArgumentException("Movie not found.");
            }
            if (movie.Tickets.Count < numTickets)
            {   
                return false;
                throw new ArgumentException("Movie does not have sufficient amount of tickets to remove. ");
            }
            movie.Tickets.RemoveRange(0, numTickets);
            _context.SaveChanges();
            return true;
        }
    }
    
    /// <summary>
    /// Database properties
    /// </summary>
    public class MovieContext : DbContext
    {

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(System.Environment.GetEnvironmentVariable("movieDb"));
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
            modelBuilder.Entity<Comment>();
        }
    }
}