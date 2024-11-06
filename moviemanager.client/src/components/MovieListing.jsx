import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './MovieListing.css';
import { useParams, useNavigate } from 'react-router-dom';
import './MovieListing.css';

const MovieListing = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [cart, setCart] = useState([]);


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

    const handleAddToCart = async (ticket) => {
      try {
          setIsLoading(true);

          // Replace with the actual cartId you are working with
          const cartId = 1; // Example cartId
          const quantity = ticket.quantity > 0 ? ticket.quantity : 1; // Ensure quantity is valid and defaults to 1

          const response = await fetch('/movie/addtickettocart', {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json',
              },
              body: JSON.stringify({
                  cartId: cartId,
                  ticketId: ticket.id,
                  quantity: quantity,
              }),
          });

          if (response.ok) {
              // Add ticket to local cart state if the API call was successful
              setCart((prevCart) => {
                  const existingTicket = prevCart.find(item => item.id === ticket.id);
                  if (existingTicket) {
                      return prevCart.map(item =>
                          item.id === ticket.id ? { ...item, quantity: item.quantity + quantity } : item
                      );
                  }
                  return [...prevCart, { ...ticket, quantity: quantity }];
              });

              alert(`Added Ticket #${ticket.id} to the cart!`);
          } else {
              const errorData = await response.json();
              alert(`Failed to add the ticket to the cart: ${errorData.message || 'Unknown error'}`);
          }
      } catch (error) {
          console.error("Error adding ticket to cart:", error);
          alert("An error occurred. Please try again later.");
      } finally {
          setIsLoading(false);
      }
  };

    const handleGoHome = () => {
        navigate('/');
    };


    return (
        <div className="movie-details">
            <h1 className="movie-title">{movie.name}</h1>
            <p className="movie-description">{movie.description}</p>
            <p className="movie-id">{movie.id}</p>
            <button onClick={handleGoHome}>Go to Home</button>

            <div className="ticket-section">
                <h3>Tickets</h3>
                {movie.tickets && movie.tickets.length > 0 ? (
                    <ul className="ticket-list">
                        {movie.tickets.map((ticket, index) => (
                            <li key={index} className="ticket-item">
                                Ticket #{index + 1}
                                <button
                                    onClick={() => handleAddToCart(ticket)}
                                    className="add-to-cart-button"
                                >
                                    Add to Cart
                                </button>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <p className="no-tickets">No tickets available</p>
                )}
            </div>
        </div>
    );
};

export default MovieListing;
