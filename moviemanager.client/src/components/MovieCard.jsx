import React from 'react';
import './MovieCard.css';

function MovieCard({ movie }) {

    return (
        <div className="movie-card" >
            {/* {movie.posterUrl && <img src={movie.posterUrl} alt={`${movie.name} poster`} className="movie-poster" />} */}
            <div className="movie-details">
                <h2 className="movie-title">{movie.name}</h2>
                <p className="movie-description">{movie.description}</p>
                <p className="movie-id">{movie.id}</p>
                <div className="ticket-section">
                    <h3>Tickets</h3>
                    {movie.tickets && movie.tickets.length > 0 ? (
                        <ul className="ticket-list">
                            {movie.tickets.map((ticket, index) => (
                                <li key={index} className="ticket-item">
                                    Ticket #{index + 1}
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="no-tickets">No tickets available</p>
                    )}
                </div>
            </div>
        </div>
    );
}

export default MovieCard;
