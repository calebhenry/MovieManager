import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const MovieListing = () => {
    const { id } = useParams();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchMovieDetails = async () => {
            try {
                const response = await fetch(`/movie/getmovie/${id}`);
                
                // Check if the response is successful
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

    return (
        <div className="movie-details">
            <h2>{movie.title}</h2>
            {/* <img src={movie.poster} alt={movie.title} /> */}
            <p>{movie.description}</p>

        </div>
    );
};

export default MovieListing;