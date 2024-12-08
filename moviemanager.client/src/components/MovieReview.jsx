import React from 'react';
import "./MovieReview.css";
import { useEffect, useState } from 'react';

const MovieReview = ({globalState, reviewId, username, comment, rating, 
    likeCount, postDate, formattedPostDate, updateLikeCount}) => {
    const [lLikeCount, setLLikeCount] = useState(likeCount);
    const { user, setUser } = globalState;
    const [liked, setLiked] = useState(false);
    const [cannotLike, setCannotLike] = useState(true);
    const [buttonText, setButtonText] = useState("Like");

    useEffect(() => {
        const effect = async () => {
            const response = await fetch(`/movie/liked/${user.id}:${reviewId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            }); 
            if (response.ok) {
                const result = await response.json();
                setLiked(result);
                setButtonText(result ? "Unlike" : "Like");
                setCannotLike(false);
            }
        };
        effect();
    }, []);

    const onLike = async () => {
        if (!liked) {
            const response = await fetch(`/movie/like/${user.id}:${reviewId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            }); 
            if (response.ok) {
                updateLikeCount(reviewId, lLikeCount + 1);
                setLLikeCount(lLikeCount + 1);
                setButtonText("Unlike");
                setLiked(true);
            } else {
                alert("Failed to like review.");
            }
        } else {
            const response = await fetch(`/movie/removelike/${user.id}:${reviewId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            }); 
            if (response.ok) {
                updateLikeCount(reviewId, lLikeCount - 1);
                setLLikeCount(lLikeCount - 1);
                setButtonText("Like");
                setLiked(false);
            } else {
                alert("Failed to unlike review.");
            }
        }
    };

    return (
        <div className="review-listing">
                <div className="left">
                    <strong className="username">{username}</strong>
                </div>
                <div className="column-allways move-it-move-it">
                    <p>Rating: {rating}</p>
                    <p>Likes: {lLikeCount}</p>
                    <p>{formattedPostDate}</p>
                </div>
                <div className="left move-it-move-it">
                    <p>{comment}</p>
                </div>
                <button disabled={cannotLike} onClick={()=>{onLike();}}>{buttonText}</button>
        </div>
    );
};

export default MovieReview;
