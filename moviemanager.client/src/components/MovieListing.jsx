import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './MovieListing.css';

const MovieListing = ({ globalState }) => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { cart, setCart } = globalState;

    useEffect(() => {
        const fetchMovieDetails = async () => {
            try {
                const response = await fetch(`/movie/getmovie/${id}`);

                if (!response.ok) {
                    throw new Error(`Server error: ${response.statusText}`);
                }

                const data = await response.json();
                setMovie(data);
            } catch (error) {
                setError('Failed to load movie details: ' + error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchMovieDetails();
    }, [id]);

    if (loading) return <p>Loading movie details...</p>;
    if (error) return <p>{error}</p>;

    const handleAddToCart = async (ticket, add) => {
        if (cart == null) {
            try {
                const response = await fetch('/movie/getcart');
                const data = await response.json();
                setCart(data);
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            }
        }

        var quantity = GetQuantity(ticket.id);

        if (add) {
            quantity++;
        } else {
            quantity--;
        }

        if (quantity < 0) {
            quantity = 0;
        }

        const response = await fetch('/movie/addtickettocart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                cartId: cart.id,
                ticketId: ticket.id,
                quantity: quantity,
                movieId: id
            }),
        });


        if (response.ok) {
            try {
                const response = await fetch(`/movie/getcart?cartId=${encodeURIComponent(cart.id)}`);
                const data = await response.json();
                setCart(data);
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            }
        } else {
            const errorData = await response.json();
            alert(`Failed to add the ticket to the cart: ${errorData.message || 'Unknown error'}`);
        }
    };

    const handleGoHome = () => {
        navigate('/home');
    };

    const GetQuantity = (ticketId) => {
        if (cart != null) {
            const num = cart.tickets.find(item => item.ticketId === ticketId && item.ticket.movieId == id)?.quantity;
            return num ?? 0;
        } else {
            return 0;
        }
    }

    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        return `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')} ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    }

    return (
        <div className="screen">
            <div className="movie-details-cont">
                <h1 className="movie-title">{movie.name}</h1>
                <p className="movie-description">{movie.description}</p>
                <div className="ticket-section">
                    <h3>Tickets</h3>
                    {movie.tickets && movie.tickets.length > 0 ? (
                        <ul className="ticket-list">
                            {movie.tickets.map((ticket, index) => (
                                <li key={index} className="ticket-item">
                                    <span>{formatDateTime(ticket.showtime)}</span>
                                    <div className="quantity-controls">
                                        <button onClick={() => handleAddToCart(ticket, false)}>-</button>
                                        <span>{GetQuantity(ticket.id)}</span>
                                        <button onClick={() => handleAddToCart(ticket, true)}>+</button>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="no-tickets">No tickets available</p>
                    )}
                </div>
                <button onClick={handleGoHome}>Go to Home</button>
            </div>
        </div>
    );
};

export default MovieListing;