import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import './Manager.css';

const Manager = () => {
  const [movies, setMovies] = useState([]);
  const [tickets, setTickets] = useState([]);
  const [newMovie, setNewMovie] = useState({ name: "", description: "", genre: "ACTION" });
  const [newTicket, setNewTicket] = useState({ movieId: "", showtime: "", price: "", numAvailable: "" });

  useEffect(() => {
    fetchMovies();
    fetchTickets();
  }, []);

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

  const fetchTickets = async () => {
    try {
      const response = await fetch("/movie/gettickets");
      if (!response.ok) throw new Error("Failed to fetch tickets");
      const data = await response.json();
      setTickets(data);
    } catch (error) {
      console.error("Error fetching tickets:", error);
    }
  };

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
  
  

  const handleUpdateMovie = async (movie) => {
    try {
      const { id, name, description, genre } = movie; // Keep only essential properties
      const updatedMovie = { id, name, description, genre };
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

  const handleAddTicket = async () => {
    try {
      const response = await fetch("/movie/addticket", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(newTicket)
      });
      if (!response.ok) throw new Error("Failed to add ticket");
      alert("Ticket added successfully!");
      fetchTickets();
    } catch (error) {
      console.error("Error adding ticket:", error);
    }
  };

  const handleUpdateTicket = async (id, updatedTicket) => {
    try {
      const response = await fetch(`/api/tickets/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(updatedTicket)
      });
      if (!response.ok) throw new Error("Failed to update ticket");
      alert("Ticket updated successfully!");
      fetchTickets();
    } catch (error) {
      console.error("Error updating ticket:", error);
    }
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
                <button className="manager-button" onClick={() => handleUpdateMovie(movie)}>Save Changes</button>
                <button className="manager-button manager-remove-button" onClick={() => handleRemoveMovie(movie)}>Remove Movie</button>
              </div>
            ))}
          </div>
        </div>
  
        {/* Add New Ticket */}
        <div className="manager-grid-item">
          <h2>Add New Ticket</h2>
          <input
            className="manager-input"
            type="number"
            placeholder="Movie ID"
            value={newTicket.movieId}
            onChange={(e) => setNewTicket({ ...newTicket, movieId: e.target.value })}
          />
          <input
            className="manager-input"
            type="datetime-local"
            placeholder="Showtime"
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
            placeholder="Number Available"
            value={newTicket.numAvailable}
            onChange={(e) => setNewTicket({ ...newTicket, numAvailable: e.target.value })}
          />
          <button className="manager-button" onClick={handleAddTicket}>Add Ticket</button>
        </div>
  
        {/* Tickets List */}
        <div className="manager-grid-item">
          <h2>Tickets</h2>
          <div className="manager-tickets-grid">
            {tickets.map((ticket) => (
              <div key={ticket.id} className="manager-ticket-card">
                <input
                  className="manager-input"
                  type="number"
                  value={ticket.price}
                  onChange={(e) =>
                    setTickets(tickets.map((t) => (t.id === ticket.id ? { ...t, price: e.target.value } : t)))
                  }
                />
                <input
                  className="manager-input"
                  type="number"
                  value={ticket.numAvailable}
                  onChange={(e) =>
                    setTickets(tickets.map((t) => (t.id === ticket.id ? { ...t, numAvailable: e.target.value } : t)))
                  }
                />
                <button className="manager-button" onClick={() => handleUpdateTicket(ticket.id, ticket)}>Save Changes</button>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );  
};

export default Manager;
