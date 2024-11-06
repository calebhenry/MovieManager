import React from 'react';
import './MovieCard.css';
import { Link } from 'react-router-dom';

const MovieCard = ({ movie }) => {

    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        return `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')} ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    }

    return (
        <Link to={`/movies/${movie.id}`} >
            <div className="movie-card" >
            {/* {movie.posterUrl && <img src={movie.posterUrl} alt={`${movie.name} poster`} className="movie-poster" />} */}
                <div className="movie-details">
                    <h2 className="movie-title">{movie.name}</h2>
                    <p className="movie-description">{movie.description}</p>
                    <div className="ticket-section">
                        <h3>Showtimes</h3>
                        {movie.tickets && movie.tickets.length > 0 ? (
                            <ul className="ticket-list">
                                {movie.tickets.map((ticket, index) => (
                                    <li key={index} className="ticket-item">
                                        Showtime #{index + 1}: { formatDateTime(ticket.showtime) }
                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p className="no-tickets">No showtimes available</p>
                        )}
                    </div>
                </div>
            </div>
        </Link>
    );
}

export default MovieCard;