import React, { useState, useEffect } from 'react';
import MovieCard from './MovieCard';
import '../components/Home.css';
import { Link, useNavigate } from 'react-router-dom';

const Home = ({ globalState }) => {
    const { movies, setMovies, user, cart, setCart } = globalState;
    const [rating, setRating] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => { 
        const fetchMovies = async () => {
            try {
                const response = await fetch('movie/getmovies');
                const data = await response.json();
                setMovies(data);
            } catch (error) {
                setError('Failed to fetch movies. Please try again later.');
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

    const handleGoCart = () => {
        navigate('/cart');
    };

    const handleGoSettings = () => {
        navigate('/settings');
    };

    const handleGoManage = () => {
        navigate('/manager');
    };

    const handleRatingChange = (e) => {
        setRating(e.target.value);
    }

    const meetsRating = (movie) => {
        switch (rating) {
            case 'G':
                return movie.ageRating < 9;
            case 'PG':
                return movie.ageRating >= 9 && movie.ageRating < 13;
            case 'PG-13':
                return movie.ageRating >= 13 && movie.ageRating < 18;
            case 'R':
                return movie.ageRating >= 18;
            default:
                return true;
        }
    }

    // Filter by genre
    const filteredMoviesByGenre = movies.filter(m => {
        console.log("Sort" + meetsRating(m));
        return meetsRating(m);
    }).reduce((acc, movie) => {
        const genre = movie.genre;
        if (!acc[genre]) {
            acc[genre] = [];
        }
        acc[genre].push(movie);
        return acc;
    }, {});

    if (loading) {
        return <p>Loading movies...</p>;
    }
    else {
        return (
            <div className="body">
                <div className="nav">
                    <br></br><br></br><h1>Welcome to Movie Browser {user.name}!</h1>
                    <div className="bar">
                        <button onClick={handleGoSettings}>Settings</button>
                        {/* Default to Cart button if permission is not ADMIN */}
                        {user.permissionLevel === 1 ? (
                            <button onClick={handleGoManage}>Manage</button>
                        ) : (
                            <button onClick={handleGoCart}>Cart</button>
                        )}
                    </div>
                </div>
                <div className="home">
                    <div className="filter">
                        <h3>Filter:</h3>
                        <select value={rating} onChange={handleRatingChange}>
                            <option value="">All Ratings</option>
                            <option value="G">G</option>
                            <option value="PG">PG</option>
                            <option value="PG-13">PG-13</option>
                            <option value="R">R</option>
                        </select>
                    </div>
                    {movies.length > 0 ? (
                        Object.keys(filteredMoviesByGenre).map((genre) => (
                            <div key={genre} className="genre-section">
                                <h2>{genre}</h2>
                                <div className="movie-grid">
                                    {filteredMoviesByGenre[genre].map((movie) => (
                                        <MovieCard key={movie.id} movie={movie} />
                                    ))}
                                </div>
                            </div>
                        ))
                    ) : (
                        <p>No movies available</p>
                    )}
                </div>
            </div>
        );
    }
}

export default Home;