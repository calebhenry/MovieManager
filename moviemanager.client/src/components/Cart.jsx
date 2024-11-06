import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Cart.css';

const Cart = ({ globalState }) => {
    const navigate = useNavigate();
    const { cart, setCart, movies } = globalState;
    const [total, setTotal] = useState(0);

    useEffect(() => {
        if (cart != null) {
            setTotal(cart.tickets.reduce((sum, item) => sum + item.ticket.price * item.quantity, 0));
        }
    }, [cart])

    // Remove item from cart
    const handleRemove = async (ticketId, movieId) => {
        const response = await fetch(`movie/removeticketfromcart?cartId=${cart.id}&ticketId=${ticketId}&movieId=${movieId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            try {
                const response = await fetch(`/movie/getcart?cartId=${encodeURIComponent(cart.id)}`);
                const data = await response.json();
                setCart(data);
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            }
        } else {
            const errorData = await response.json();
            alert(`Failed to add the ticket to the cart: ${errorData.message || 'Unknown error'}`);
        }
    };

    // Increase ticket quantity
    const handleIncreaseQuantity = async (ticketId, movieId, add) => {
        if (cart == null) {
            try {
                const response = await fetch('/movie/getcart');
                const data = await response.json();
                setCart(data);
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            } finally {
                setLoading(false);
            }
        }

        var quantity = GetQuantity(ticketId, movieId);

        if (add) {
            quantity++;
        } else {
            quantity--;
        }

        if (quantity < 0) {
            quantity = 0;
        }

        const response = await fetch('/movie/addtickettocart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                cartId: cart.id,
                ticketId: ticketId,
                quantity: quantity,
                movieId: movieId
            }),
        });


        if (response.ok) {
            try {
                const response = await fetch(`/movie/getcart?cartId=${encodeURIComponent(cart.id)}`);
                const data = await response.json();
                setCart(data);
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            }
        } else {
            const errorData = await response.json();
            alert(`Failed to add the ticket to the cart: ${errorData.message || 'Unknown error'}`);
        }
    };

    const GetQuantity = (ticketId, movieId) => {
        if (cart != null) {
            const num = cart.tickets.find(item => item.ticketId === ticketId && item.ticket.movieId == movieId)?.quantity;
            return num ?? 0;
        } else {
            return 0;
        }
    }

    // Proceed to payment
    const handleProceedToPayment = () => {
        navigate('/payment');
    };

    const handleGoHome = () => {
        navigate('/home');
    };

    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        return `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')} ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    }

    const getMovieName = (movieId) => {
        var title = movies.find(movie => movie.id === movieId).name;
        if (title.length > 15) {
            title = title.substring(0, 15) + "...";
        }
        return title;
    }

    return (
        <div className="screen">
            <div className="cart-container">
                <h1>Your Cart</h1>
                {cart == null || cart.tickets.length == 0 ? (
                    <p>Your cart is empty.</p>
                ) : (
                    <>
                        <ul className="cart-list">
                            {cart.tickets.map((item, index) => (
                                <li key={index} className="cart-item">
                                    <span>{getMovieName(item.ticket.movieId)}</span>
                                    <span>{formatDateTime(item.ticket.showtime)}</span>
                                    <span>${item.ticket.price.toFixed(2)}</span>
                                    <div className="quantity-controls">
                                        <button onClick={() => handleIncreaseQuantity(item.ticketId, item.ticket.movieId, false)}>-</button>
                                        <span>{item.quantity}</span>
                                        <button onClick={() => handleIncreaseQuantity(item.ticketId, item.ticket.movieId, true)}>+</button>
                                    </div>
                                    <span>${(item.ticket.price * item.quantity).toFixed(2)}</span>
                                    <button onClick={() => handleRemove(item.ticketId, item.ticket.movieId)}>Remove</button>
                                </li>
                            ))}
                        </ul>
                            <div className="cart-total">
                                <strong>Total:</strong> ${total.toFixed(2)}
                        </div>
                    </>
                )}
                <div>
                    <button onClick={handleGoHome} style={{ margin: 10 }}>Go to Home</button>
                    <button onClick={handleProceedToPayment} style={{ margin: 10 }}>
                        Proceed to Payment
                    </button>
                </div>
            </div>
        </div>
    );
};

export default Cart;
