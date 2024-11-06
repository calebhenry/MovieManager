import React, { useState, useEffect } from 'react';
import MovieCard from './MovieCard';
import '../components/Home.css';
import { Link, useNavigate } from 'react-router-dom';

const Home = ({ globalState }) => {
    const { movies, setMovies, user, cart, setCart } = globalState;
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null); // For error state handling
    const navigate = useNavigate();

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
        const fetchCart = async () => {
            if (cart == null) {
                try {
                    const response = await fetch('/movie/getcart');
                    const data = await response.json();
                    setCart(data);
                } catch (error) {
                    setError('Failed to fetch cart. Please try again later.');
                }
            }
        }
        fetchCart();
    }, []);

    if (loading) {
        return <p>Loading movies...</p>;
    }

    const handleGoCart = () => {
        navigate('/cart');
    };

    const handleGoSettings = () => {
        navigate('/settings');
    };

    return (
        <div className="body">
            <div className="nav">
                <br></br><br></br><h1>Welcome to Movie Browser {user.name}!</h1>
                <div className="bar">
                    <button onClick={handleGoSettings}>Settings</button>
                    <button onClick={handleGoCart}>Cart</button>
                </div>
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
