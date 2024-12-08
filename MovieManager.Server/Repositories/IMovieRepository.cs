﻿using MovieManager.Server.Models;

namespace MovieManager.Server.Repositories
{
    public interface IMovieRepository
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicket(Ticket ticket);
        void AddUser(User user);
        bool AddReview(Review review);
        List<Cart> GetCarts();
        void UpdateCart(Cart cart);
        Movie? GetMovieById(int id);
        List<Movie> GetMovies();
        List<Ticket> GetTickets();
        User? GetUser(string username, string password);
        List<User> GetUsers();
        bool RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        void RemoveUser(User user);
        User? UpdateUser(UpdatedUser updatedUser);
        Review? EditReview(int currentUserId, UpdatedReview updatedReview);
        Ticket EditTickets(int movieId, UpdatedTicket updatedTicket);
        List<Review> GetReviews(int movieId);
        bool RemoveReview(Review review);
    }
}