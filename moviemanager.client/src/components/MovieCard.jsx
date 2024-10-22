import React from 'react';
import './MovieCard.css';

function MovieCard({ movie }) {
    return (
        <div className="movie-card">
            <h2>{movie.name}</h2>
            <p>{movie.description}</p>
            <ul>
                {movie.tickets && movie.tickets.length > 0 ? (
                    movie.tickets.map((ticket, index) => <li key={index}>Ticket #{index + 1}</li>)
                ) : (
                    <p>No tickets available</p>
                )}
            </ul>
        </div>
    );
}

export default MovieCard;