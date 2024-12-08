import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './MovieListing.css';

const MovieListing = ({ globalState }) => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [movie, setMovie] = useState(null);
    const [showReview, setShowReview] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const { cart, setCart } = globalState;
    const [rating, setRating] = useState(3);
    const [comment, setComment] = useState("");
    const [showName, setShowName] = useState(true);
    const [addReviewText, setAddReviewText] = useState("Add Review");
    const { user, setUser } = globalState;

    const submitReview = async() => {
        const review = {
            Id: 0,
            MovieId: id,
            UserId: user.id,
            PostDate: new Date().toISOString(),
            Rating: rating,
            LikeCount: 0,
            Comment: comment,
            Anonymous: showName
        };

        const response = await fetch('/movie/addreview', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(review),
        }); 
    };

    const onAddReview = async() => {
        if (showReview) {
            submitReview();
            setShowReview(false);
            setAddReviewText("Add Review");
        } else {
            setShowReview(true);
            setAddReviewText("Post Review");
        }
    };

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
    const Error = () => {
        if (error) return <p>{error}</p>;
    }

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

        const currQuantity = GetQuantity(ticket.id);

        var quantity = 1;
        if (!add) {
            quantity = -1;
        }

        if (currQuantity + quantity < 0) {
            setError('Cannot have less than 0 tickets.');
            quantity = 0;
        }
        else
        {
            setError(null);
        }

        const response = await fetch(`/movie/addtickettocart?cartId=${cart.id}&ticketId=${ticket.id}&quantity=${quantity}`, {
            method: 'POST',
            headers: {
            },
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
            alert(`Failed to add the ticket to the cart: ${errorData.message || 'No tickets remaining'}`);
        }
    };

    const handleGoHome = () => {
        navigate('/home');
    };

    const GetQuantity = (ticketId) => {
        if (cart != null) {
            const num = cart.tickets.find(item => item.ticketId === ticketId)?.quantity;
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
                {!showReview && (
                <div className="ticket-section">
                    <h3>Tickets</h3>
                    <Error />
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
                </div>) }
                {showReview && (<>
                    <div>
                        <label for="rating">Rating: </label>
                        <input type="range" value={rating} onChange={(e) => {setRating(e.target.value);}} id="rating-range" name="rating" min="1" max="5" />
                        <label>{rating}</label>
                    </div>
                    <div>
                        <label for="comment">Comment:</label>
                    </div>
                    <div>
                        <textarea name="comment" rows="5" columns="50" onChange={(e) => {setComment(e.target.value);}}>{comment}</textarea>
                    </div>
                    <div>
                        <label for="useName">Post Anonymously</label>
                        <input type="checkbox" name="useName" onChange={(e)=>{setShowName(e.target.value == "on" ? true : false);}}/>
                    </div>
                    </>)}
                <div className="column-allways">
                    <button onClick={handleGoHome}>Go to Home</button>
                    <button onClick={() => {onAddReview();}}>{addReviewText}</button>
                </div>
            </div>
        </div>
    );
};

export default MovieListing;
