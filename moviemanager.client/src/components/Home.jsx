/**
 * @author Caleb Henry
 */

import React, { useState, useEffect } from 'react';
import MovieCard from './MovieCard';
import '../components/Home.css';
import { Link } from 'react-router-dom';

const Home = ({ globalState }) => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const { user } = globalState;

    useEffect(() => {
        const fetchMovies = async () => {
            try {
                const response = await fetch('movie/getmovies');  // Replace with your API endpoint
                const data = await response.json();
                setMovies(data);
            } catch (error) {
                console.error('Error fetching movies:', error);
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
        <div className="home">
            <h1>Movie List</h1>
            <Link to="/login">Login</Link>
            <Link to="/settings">Settings</Link>
            <h2>Welcome {user.name}!</h2>
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
    );
}

export default Home;

