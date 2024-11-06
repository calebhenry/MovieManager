import React, { useState, useEffect } from 'react';
import MovieCard from './MovieCard';
import '../components/Home.css';
import { Link } from 'react-router-dom';

const Home = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null); // For error state handling

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
    }, []);

    if (loading) {
        return <p>Loading movies...</p>;
    }

    return (
        <div className="body">
            <div className="nav">
                <br></br><br></br><h1>Welcome to Movie Broswer!</h1>
                <Link to="/payment">Go to Payment</Link> <br></br>
                <Link to="/cart">Go to Cart</Link>
            </div>
            <div className="home">
                <div className="movie-grid">
                    {movies.length > 0 ? (
                        movies.map((movie) => (
                            <MovieCard key={movie.id} movie={movie} />
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
