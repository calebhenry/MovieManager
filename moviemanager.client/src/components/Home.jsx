import React, { useState, useEffect } from 'react';
import MovieCard from './MovieCard';
import '../components/Home.css';
import { useNavigate } from 'react-router-dom';

const Home = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null); // For error state handling

    useEffect(() => {
        const fetchMovies = async () => {
            try {
                const response = await fetch('movie');  // Replace with your API endpoint
                const data = await response.json();
                setMovies(data);
            } catch (error) {
                setError('Failed to fetch movies. Please try again later.'); // Handle the error
                console.error('Error fetching movies:', error);
            } finally {
                setLoading(false);
            }
        }
        fetchMovies();
    }, []);

    const handleMovieClick = (id) => {
        navigate(`/movie/${id}`); // Navigate to the movie details page with the movie ID
    };

    if (loading) {
        return <div className="spinner">Loading movies...</div>;  // Use a CSS spinner here
    }

    if (error) {
        return <p className="error-message">{error}</p>; // Display the error message
    }

    return (
        <div className="body">
            <div className="nav">
                <h1>Movie List</h1>
            </div>
            <div className="home">
                <div className="movie-grid">
                    {movies.length > 0 ? (
                        movies.map((movie) => (
                            <MovieCard key={movie.id} movie={movie} onClick={() => handleMovieClick(movie.id)} />
                        ))
                    ) : (
                        <p>No movies available</p>
                    )}
                </div>
            </div>
        </div>
    );
    
}

export default Home;
