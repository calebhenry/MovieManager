import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

const Movie = ({ addToCart }) => {
  const { movieId } = useParams(); // Get movieId from the URL
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true); // Add loading state
  const [error, setError] = useState(null); // Add error state

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        console.log(`Fetching movie details for ID: ${movieId}`); // Debugging log to check the movieId
        const response = await fetch(`/movie/${movieId}`); // Fetch movie by movieId from URL
        
        if (!response.ok) {
          throw new Error('Failed to fetch movie details');
        }

        const data = await response.json();
        console.log('Movie data fetched:', data); // Debugging log to check the data being fetched
        setMovie(data); // Set the movie details in state
      } catch (error) {
        console.error('Error fetching movie details:', error); // Improved error logging
        setError('Failed to fetch movie details. Please try again later.');
      } finally {
        setLoading(false); // Set loading to false after API call finishes
      }
    };

    fetchMovieDetails();
  }, [movieId]); // Depend on movieId from URL

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
