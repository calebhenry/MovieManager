import React from 'react';
import "./MovieReview.css";
import MovieReply from "./MovieReply";
import { useEffect, useState } from 'react';

const MovieReview = ({globalState, reviewId, username, comment, rating, 
    likeCount, postDate, formattedPostDate, updateLikeCount}) => {

    // State Variables
    const [lLikeCount, setLLikeCount] = useState(likeCount);
    const { user, setUser } = globalState;
    const [anonChecked, setAnonChecked] = useState(false);
    const [message, setMessage] = useState("");
    const [liked, setLiked] = useState(false);
    const [cannotLike, setCannotLike] = useState(true);
    const [buttonText, setButtonText] = useState("Like");
    const [comments, setComments] = useState([]);

    // Sends request too add a comment to the review
    const onComment = async () => {
        const lComment = {
            message: message,
            id: 0,
            reviewId: reviewId,
            username: (anonChecked ? "Anon" : user.username)
        };
        const res = await fetch(`/movie/addcomment`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(lComment)
        }); 
        if (res.ok) {
            setComments([...comments, lComment]);
        } else {
            alert("Failed to add comment.");
        }
    };

    // Toggles the checkbox for posting replies anonymously
    const onChecked = () => {
        setAnonChecked(!anonChecked);
    };

    // Fetches the initial data for the review, including comments and like status
    useEffect(() => {
        const effect = async () => {
            // Fetch comments for the review
            const res = await fetch(`/movie/getcomments?reviewId=${reviewId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            }); 
            if (res.ok) {
                setComments(await res.json());
            }
            // Check if the user has liked this review
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

    // Toggles the like/unlike status of the review
    const onLike = async () => {
        if (!liked) {
            // Send request to like the review
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
            // Send request to unlike the review
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
            {/* Review header with username */}
                <div className="left">
                    <strong className="username">{username}</strong>
                </div>
                {/* Review details: rating, likes, and post date */}
                <div className="column-allways move-it-move-it">
                    <p>Rating: {rating}</p>
                    <p>Likes: {lLikeCount}</p>
                    <p>{formattedPostDate}</p>
                </div>
                {/* Review comment content */}
                <div className="left move-it-move-it">
                    <p>{comment}</p>
                </div>
                {/* Display replies to the review */}
                {(comments.length > 0) && (
                    <>
                        <h3 className="left">Replies:</h3>
                        {comments.map((i) => (
                            <MovieReply
                                key={Math.random()}
                                username={i.username}
                                message={i.message} />
                        ))}
                    </>
                )}
                {/* Buttons to like/unlike the review and to reply */}
                <button disabled={cannotLike} onClick={()=>{onLike();}}>{buttonText}</button>
                <button onClick={()=>{onComment();}}>Reply</button>
                <label for="useName">Reply Anonymously</label>
                <input type="checkbox" name="useName" checked={anonChecked} onChange={onChecked}/>
                {/* Input field for reply message */}
                <input type="text" name="comment" value={message} onChange={(e) => {setMessage(e.target.value);}}/>
        </div>
    );
};

export default MovieReview;
