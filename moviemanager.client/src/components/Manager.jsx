import React, { useState, useEffect } from "react";
import './Manager.css';

const Manager = () => {

  // Stattee variables for movies, ticketss, and form inputs
  const [movies, setMovies] = useState([]);
  const [tickets, setTickets] = useState([]);
  const [newMovie, setNewMovie] = useState({ name: "", description: "", genre: "ACTION", ageRating: 13 });
  const [newTicket, setNewTicket] = useState(null);
  const [selectedMovieId, setSelectedMovieId] = useState(null);
  const [editTicket, setEditTicket] = useState(null);

  // Fetch movies
  useEffect(() => {
    fetchMovies();
  }, []);

  // Fetch tickets when the lisst of movies changes
  useEffect(() => {
    if (movies.length > 0) {
      fetchTickets();
    }
  }, [movies]);

  // Fetch all movies from the API
  const fetchMovies = async () => {
    try {
      const response = await fetch("/movie/getmovies");
      if (!response.ok) throw new Error("Failed to fetch movies");
      const data = await response.json();
      setMovies(data);
    } catch (error) {
      console.error("Error fetching movies:", error);
    }
  };

  // Fetch all tickets from the API
  const fetchTickets = () => {
    try {
      const allTickets = movies.flatMap(movie => movie.tickets);
      setTickets(allTickets);
    } catch (error) {
      console.error("Error processing tickets:", error);
    }
  };

  // Adds a new movie, updated in database
  const handleAddMovie = async () => {
    try {
      const response = await fetch("/movie/addmovie", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(newMovie)
      });
      if (!response.ok) throw new Error("Failed to add movie");
      alert("Movie added successfully!");
      fetchMovies();
    } catch (error) {
      console.error("Error adding movie:", error);
    }
  };

  // Remove Movie, updated in database
  const handleRemoveMovie = (movie) => {
    fetch(`/movie/removemovie/`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(movie)
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to delete movie");
        }
        alert("Movie removed successfully!");
        fetchMovies();
      })
      .catch((error) => {
        console.error("Error removing movie:", error);
      });
  };

  // Updates an existing movie's information
  const handleUpdateMovie = async (movie) => {
    try {
      const { id, name, description, genre, ageRating } = movie; // Keep only essential properties
      const updatedMovie = { id, name, description, genre, ageRating };
      const response = await fetch("/movie/editmovie", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedMovie),
      });
      if (!response.ok) throw new Error("Failed to update movie");
      alert("Movie updated successfully!");
      fetchMovies();
    } catch (error) {
      console.error("Error updating movie:", error);
    }
  };

  // Adds a ticket to a movie given movie ID
  const handleAddTicket = async () => {
    try {
      const response = await fetch("/movie/addticketstomovie", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ ...newTicket, movieId: selectedMovieId })
      });
      if (!response.ok) throw new Error("Failed to add ticket");
      alert("Ticket added successfully!");
      fetchMovies();
      setNewTicket(null);
    } catch (error) {
      console.error("Error adding ticket:", error);
    }
  };

  // Update Ticket details
  const handleUpdateTicket = async (ticket) => {
    try {
      const response = await fetch(`/movie/edittickets?movieId=${ticket.movieId}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(ticket)
      });
      if (!response.ok) throw new Error("Failed to update ticket");
      alert("Ticket updated successfully!");
      setEditTicket(null);
      fetchMovies();
    } catch (error) {
      console.error("Error updating ticket:", error);
    }
  };

  // Deletes a Ticket from a movie
  const handleDeleteTicket = async (ticket) => {
    try {
      const response = await fetch(`/movie/removeticketfrommovie`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(ticket)
      });
      if (!response.ok) throw new Error("Failed to delete ticket");
      alert("Ticket deleted successfully!");
      setEditTicket(null);
      fetchMovies();
    } catch (error) {
      console.error("Error deleting ticket:", error);
    }
  };

  const openTicketWindow = (movieId) => {
    setSelectedMovieId(movieId);
    setEditTicket(null);
    setNewTicket(null);
  };

  const openEditTicketWindow = (ticket) => {
    setEditTicket(ticket);
    setNewTicket(null);
  };

  const openAddTicketWindow = () => {
    setNewTicket({ showtime: "", price: "", numAvailible: "", movieId: selectedMovieId });
    setEditTicket(null);
  };


  return (
    <div className="manager-screen">
      <h1 className="manager-h1">Manager Page</h1>

      <div className="manager-grid-container">
        {/* Add New Movie */}
        <div className="manager-grid-item">
          <h2>Add New Movie</h2>
          <input
            className="manager-input"
            type="text"
            placeholder="Name"
            value={newMovie.name}
            onChange={(e) => setNewMovie({ ...newMovie, name: e.target.value })}
          />
          <textarea
            className="manager-textarea"
            placeholder="Description"
            value={newMovie.description}
            onChange={(e) => setNewMovie({ ...newMovie, description: e.target.value })}
          ></textarea>
          <select
            className="manager-select"
            value={newMovie.genre}
            onChange={(e) => setNewMovie({ ...newMovie, genre: e.target.value })}
          >
            <option value="ACTION">Action</option>
            <option value="COMEDY">Comedy</option>
            <option value="DRAMA">Drama</option>
            <option value="HORROR">Horror</option>
            <option value="ROMANCE">Romance</option>
            <option value="THRILLER">Thriller</option>
          </select>
          <input
            className="manager-input"
            type="number"
            placeholder="Age Rating"
            value={newMovie.ageRating}
            onChange={(e) => setNewMovie({ ...newMovie, ageRating: e.target.value })}
          />
          <button className="manager-button" onClick={handleAddMovie}>Add Movie</button>
        </div>

        {/* Movie List */}
        <div className="manager-grid-item">
          <h2>Movies</h2>
          <div className="manager-movies-grid">
            {movies.map((movie) => (
              <div key={movie.id} className="manager-movie-card">
                <input
                  className="manager-input"
                  type="text"
                  value={movie.name}
                  onChange={(e) =>
                    setMovies(movies.map((m) => (m.id === movie.id ? { ...m, name: e.target.value } : m)))
                  }
                />
                <textarea
                  className="manager-textarea"
                  value={movie.description}
                  onChange={(e) =>
                    setMovies(movies.map((m) => (m.id === movie.id ? { ...m, description: e.target.value } : m)))
                  }
                ></textarea>
                <select
                  className="manager-select"
                  value={movie.genre}
                  onChange={(e) =>
                    setMovies(movies.map((m) => (m.id === movie.id ? { ...m, genre: e.target.value } : m)))
                  }
                >
                  <option value="ACTION">Action</option>
                  <option value="COMEDY">Comedy</option>
                  <option value="DRAMA">Drama</option>
                  <option value="HORROR">Horror</option>
                  <option value="ROMANCE">Romance</option>
                  <option value="THRILLER">Thriller</option>
                </select>
                <input
                  className="manager-input"
                  type="number"
                  placeholder="Age Rating"
                  value={movie.ageRating}
                  onChange={(e) =>
                    setMovies(movies.map((m) => (m.id === movie.id ? { ...m, ageRating: e.target.value } : m)))
                  }
                />
                <button className="manager-button" onClick={() => handleUpdateMovie(movie)}>Update Movie</button>
                <button className="manager-button" onClick={() => handleRemoveMovie(movie)}>Remove Movie</button>
                <button className="manager-button" onClick={() => openTicketWindow(movie.id)}>Edit Tickets</button>
              </div>
            ))}
          </div>
        </div>

        {/* Ticket list */}
        {selectedMovieId && (
          <div className="manager-grid-item">
            <h2>Tickets for Selected Movie</h2>
            <div className="manager-tickets-grid">
              {tickets.filter((ticket) => ticket.movieId == selectedMovieId).map((ticket) => (
                <div key={ticket.id} className="manager-ticket-card">
                  <p>Showtime: {ticket.showtime}</p>
                  <p>Price: {ticket.price}</p>
                  <p>Quantity Available: {ticket.numAvailible}</p>
                  <button className="manager-button" onClick={() => openEditTicketWindow(ticket)}>Edit Ticket</button>
                  <button className="manager-button" onClick={() => handleDeleteTicket(ticket)}>Delete Ticket</button>
                </div>
              ))}
            </div>
            <button className="manager-button" onClick={() => openAddTicketWindow()}>Add Ticket</button>
          </div>
        )}

        {/* Edit Ticket Window */}
        {editTicket && (
          <div className="manager-grid-item">
            <h2>Edit Ticket</h2>
            <input
              className="manager-input"
              type="datetime-local"
              value={editTicket.showtime}
              onChange={(e) => setEditTicket({ ...editTicket, showtime: e.target.value })}
            />
            <input
              className="manager-input"
              type="number"
              placeholder="Price"
              value={editTicket.price}
              onChange={(e) => setEditTicket({ ...editTicket, price: e.target.value })}
            />
            <input
              className="manager-input"
              type="number"
              placeholder="Quantity Available"
              value={editTicket.numAvailible}
              onChange={(e) => setEditTicket({ ...editTicket, numAvailible: e.target.value })}
            />
            <button className="manager-button" onClick={() => handleUpdateTicket(editTicket)}>Update Ticket</button>
            <button className="manager-button" onClick={() => handleDeleteTicket(editTicket)}>Delete Ticket</button>
          </div>
        )}

        {/* Add Ticket Window */}
        {newTicket && (
          <div className="manager-grid-item">
            <h2>Add New Ticket</h2>
            <input
              className="manager-input"
              type="datetime-local"
              value={newTicket.showtime}
              onChange={(e) => setNewTicket({ ...newTicket, showtime: e.target.value })}
            />
            <input
              className="manager-input"
              type="number"
              placeholder="Price"
              value={newTicket.price}
              onChange={(e) => setNewTicket({ ...newTicket, price: e.target.value })}
            />
            <input
              className="manager-input"
              type="number"
              placeholder="Quantity Available"
              value={newTicket.numAvailible}
              onChange={(e) => setNewTicket({ ...newTicket, numAvailible: e.target.value })}
            />
            <button className="manager-button" onClick={handleAddTicket}>Add Ticket</button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Manager;