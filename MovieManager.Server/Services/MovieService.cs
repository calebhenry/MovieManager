﻿using MovieManager.Server.Models;
using MovieManager.Server.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MovieManager.Server.Services
{
    public class MovieService : IMovieService
    {
        private IMovieRepository movieRepository;
        private User? currentUser;

        public MovieService(IMovieRepository repository)
        {
            movieRepository = repository;
        }

        public List<Movie> GetMovies()
        {
            return movieRepository.GetMovies();
        }

        public void AddMovie(Movie movie)
        {
            foreach(var ticket in movie.Tickets)
            {
                ticket.MovieId = movie.Id;
                ticket.Movie = movie;
            }
            movieRepository.AddMovie(movie);
        }

        public void RemoveTicket(Ticket ticket)
        {
            movieRepository.RemoveTicket(ticket);
        }

        public void AddCart(Cart cart)
        {
            movieRepository.AddCart(cart);
        }

        public Cart? RemoveTicket(int ticketId, int cartId)
        {
            foreach (var cart in movieRepository.GetCarts())
            {
                if (cart.Id == cartId)
                {
                    foreach (var ticket in cart.Tickets)
                    {
                        if (ticket.TicketId == ticketId)
                        {
                            cart.Tickets.Remove(ticket);
                            return cart;
                        }
                    }
                    return cart;
                }
            }
            return null;
        }

        public bool AddTicketToCart(int cartId, int ticketId, int quantity)
        {
            var carts = (from i in movieRepository.GetCarts() where i.Id == cartId select i).ToList();
            if (carts.Count == 0)
            {
                return false;
            }
            Cart cart = carts.First();
            var tickets = (from i in movieRepository.GetTickets() where i.Id == ticketId select i).ToList();
            if (tickets.Count == 0)
            {
                return false;
            }
            Ticket ticket = tickets.First();
            // assuming we decrement numAvailable only after they checkout
            var ticketsInCarts = movieRepository.GetCarts().SelectMany(c => c.Tickets).Where(t => t.TicketId == ticket.Id).Sum(item => item.Quantity);
            if (ticket.NumAvailible < quantity + ticketsInCarts)
            {
                return false;
            }
            if (!cart.Tickets.Exists(t => t.TicketId == ticket.Id))
            {
                var cartItem = new CartItem
                {
                    Id = cart.Tickets.Count,
                    CartId = cart.Id,
                    TicketId = ticket.Id,
                    Quantity = 0,
                    Cart = cart,
                    Ticket = ticket
                };
                cart.Tickets.Add(cartItem);
            }
            cart.Tickets.First(t => t.TicketId == ticket.Id).Quantity += quantity;
            return true;
        }

        public void RemoveMovie(Movie movie)
        {
            movieRepository.RemoveMovie(movie);
        }

        public User AddUser(User user)
        {
            user.Id = movieRepository.GetUsers().Count;
            movieRepository.AddUser(user);
            return user;
        }

        public User? GetUser(string username, string password)
        {
            return movieRepository.GetUser(username, password);
        }

        public User UpdateUser(UpdatedUser updatedUser)
        {
            Console.WriteLine("Updating user");
            return movieRepository.UpdateUser(updatedUser);
        }

        public void RemoveUser(User user)
        {
            movieRepository.RemoveUser(user);
        }

        public void ProcessPayment(int cartId, string cardNumber, string exp, string cardholderName, string cvc)
        {
            movieRepository.ProcessPayment(cartId, cardNumber, exp, cardholderName, cvc);
        }
        public IEnumerable<Ticket> GetTickets(int movieId)
        {
            var movie = movieRepository.GetMovies().FirstOrDefault(m => m.Id == movieId);
            return movie?.Tickets ?? Enumerable.Empty<Ticket>();
        }
        public Cart? GetCart(int cartId)
        {
            return movieRepository.GetCarts().FirstOrDefault(c => c.Id == cartId);
        }
    }
}
