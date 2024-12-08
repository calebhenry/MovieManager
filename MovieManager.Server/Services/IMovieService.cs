﻿using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicketToCart(int cartId, int ticketId, int quantity);
        User AddUser(User user);
        Cart GetCart(int? cartId);
        Movie? GetMovieById(int id);
        void AddTicket(Ticket ticket);
        int AddReview(Review review);
        bool RemoveLike(int userId, int reviewId);
        bool Liked(int userId,  int reviewId);
        bool AddLike(int userId,  int reviewId);
        List<Movie> GetMovies();
        List<Ticket> GetAllTickets();
        IEnumerable<Ticket> GetTickets(int movieId);
        User? GetUser(string username, string password);
        void ProcessPayment(int cartId, string streetAddress, string city, string state, string zipCode, string cardNumber, string exp, string cardholderName, string cvc);
        bool RemoveMovie(Movie movie);
        void RemoveTicket(Ticket ticket);
        Cart? RemoveTicketFromCart(int ticketId, int cartId);
        void RemoveUser(User user);
        User UpdateUser(UpdatedUser updatedUser);
        Review EditReview(UpdatedReview updatedReview);
        void RemoveTicketsFromMovie (int movieId, int numTickets);
        public Ticket EditTickets(UpdatedTicket updatedTicket);
        Movie EditMovie(UpdatedMovie updatedMovie);
        List<Review> GetReviews(int movieId);
        void AddTicketsToMovie(Ticket ticket);
        
        void RemoveTicketFromMovie (int movieId, int NumAvailible);
        void RemoveReview(Review review);
    }
}