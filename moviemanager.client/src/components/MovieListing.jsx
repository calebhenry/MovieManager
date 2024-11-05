import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

const MovieListing = () => {
  const { movieId } = useParams(); // Extract movieId from the URL
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true); 
  const [error, setError] = useState(null); 

  useEffect(() => {
    const fetchMovie = async () => {
        try {
            const response = await fetch(`movie/getmovie/${movieId}`);
            console.log(movieId); // Outputs the movieId (like "1") to the console
            if (!response.ok) throw new Error('Network response was not ok');
            const data = await response.json();
            setMovie(data);
        } catch (error) {
            setError('Failed to fetch movie details. Please try again later.');
        } finally {
            setLoading(false);
        }
    };
    fetchMovie();
  }, [movieId]); // Depend on movieId from URL

  // Conditional rendering based on states
  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

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

export default MovieListing;
