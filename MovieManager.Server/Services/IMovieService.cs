﻿using MovieManager.Server.Models;

namespace MovieManager.Server.Services
{
    public interface IMovieService
    {
        void AddCart(Cart cart);
        void AddMovie(Movie movie);
        bool AddTicketToCart(int cartId, int ticketId, int quantity);
        List<Movie> GetMovies();
        void RemoveMovie(Movie movie);
        Cart? RemoveTicket(int ticketId, int cartId);
        void RemoveTicket(Ticket ticket);
    }
}