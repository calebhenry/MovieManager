import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

const Movie = ({ addToCart }) => {
  const { movieId } = useParams(); // Get movieId from the URL
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true); // Add loading state
  const [error, setError] = useState(null); // Add error state

  useEffect(() => {
    const fetchMovies = async () => {
        try {
            const response = await fetch('movie/getmovies'); 
            const data = await response.json();
            setMovies(data);
        } catch (error) {
            setError('Failed to fetch movies. Please try again later.'); // Handle the error
        } finally {
            setLoading(false);
        }
    }
    fetchMovies();
  }, []); // Depend on movieId from URL

  // Conditional rendering based on states
  if (loading) return <p>Loading...</p>; // Show loading message while fetching
  if (error) return <p>{error}</p>; // Show error message if something goes wrong

  return (
    <div className="movie-details">
      {movie ? (
        <>
          <h1>{movie.title}</h1>
          <p>{movie.description}</p>
          <h3>Available Showtimes</h3>
          <ul>
            {movie.showtimes.map(showtime => (
              <li key={showtime.id}>
                {showtime.time} - ${showtime.price}
                <button onClick={() => addToCart(showtime.id)}>Add to Cart</button>
              </li>
            ))}
          </ul>
        </>
      ) : (
        <p>Movie details not found.</p>
      )}
    </div>
  );
};

export default Movie;
