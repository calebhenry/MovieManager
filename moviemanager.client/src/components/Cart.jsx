import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Cart.css';

const Cart = ({ globalState }) => {
    const navigate = useNavigate();
    const { user, cart, setCart, movies } = globalState;

    // State variables for total, discount, tax, total afteer tax, and error messages
    const [total, setTotal] = useState(0);
    const [discount, setDiscount] = useState(0);
    const [tax, setTax] = useState(0);
    const [totalAfterTax, setTotalAfterTax] = useState(0);
    const [error, setError] = useState(null);

    // Calculate totals, discount, tax, and totatl aftere tax whenever the cart changes
    useEffect(() => {
        if (cart != null) {
            const tempTotal = cart.tickets.reduce((sum, item) => sum + item.ticket.price * item.quantity, 0);
            var tempDiscount = 0;
            
            // 10% discount for senior citizens
            if(user.age >= 65) {
                tempDiscount = tempTotal*0.1;
            }
            const tempTax = (tempTotal-tempDiscount)*0.07;
            const tempTotalAfterTax = tempTotal-tempDiscount+tempTax;

            // Update state with calculated values
            setTotal(tempTotal);
            setDiscount(tempDiscount);
            setTax(tempTax);
            setTotalAfterTax(tempTotalAfterTax);
        }
    }, [cart])

    // Remove item from cart
    const handleRemove = async (ticketId) => {
        const response = await fetch(`movie/removeticketfromcart?cartId=${cart.id}&ticketId=${ticketId}`, {
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
    const handleIncreaseQuantity = async (ticketId, add) => {
        if (cart == null) {
            try {
                const response = await fetch('/movie/getcart');
                const data = await response.json();
                setCart(data); // Initialize cart if null
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            }
        }

        const currQuantity = GetQuantity(ticketId);

        // Determine if adding or removing a ticket
        var quantity = 1;
        if (!add) {
            quantity = -1;
        }

        if (currQuantity + quantity < 0) {
            setError('Cannot have less than 0 tickets.');
            quantity = 0; // Prevent quantity from going negative
        }
        else {
            setError(null);
        }

        const response = await fetch(`/movie/addtickettocart?cartId=${cart.id}&ticketId=${ticketId}&quantity=${quantity}`, {
            method: 'POST',
            headers: {
            },
        });

        if (response.ok) {
            try {
                const response = await fetch(`/movie/getcart?cartId=${encodeURIComponent(cart.id)}`);
                const data = await response.json();
                setCart(data); // Update cartt with new datta
            } catch (error) {
                setError('Failed to fetch cart. Please try again later.');
            }
        } else {
            const errorData = await response.json();
            alert(`Failed to add the ticket to the cart: ${errorData.message || 'No tickets remaining'}`);
        }
    };

    // Get the quantity of a specific ticket in the car
    const GetQuantity = (ticketId) => {
        if (cart != null) {
            const num = cart.tickets.find(item => item.ticketId === ticketId)?.quantity;
            return num ?? 0; // Returns 0 if the ticket is not found
        } else {
            return 0;
        }
    }

    // Proceed to payment
    const handleProceedToPayment = () => {
        navigate('/payment');
    };

    // Nagivate back to home page
    const handleGoHome = () => {
        navigate('/home');
    };

    // Format a date string into MM/DD HH:MM format
    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        return `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')} ${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    }

    // Get the name of a moviee by its ID
    const getMovieName = (movieId) => {
        var title = movies.find(movie => movie.id === movieId).name;
        if (title.length > 15) {
            title = title.substring(0, 15) + "...";
        }
        return title;
    }

    // Error message
    const Error = () => {
        if (error) return <p>{error}</p>;
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
                            <Error />
                            {cart.tickets.map((item, index) => (
                                <li key={index} className="cart-item">
                                    <span>{getMovieName(item.ticket.movieId)}</span>
                                    <span>{formatDateTime(item.ticket.showtime)}</span>
                                    <span>${item.ticket.price.toFixed(2)}</span>
                                    <div className="quantity-controls">
                                        <button onClick={() => handleIncreaseQuantity(item.ticketId, false)}>-</button>
                                        <span>{item.quantity}</span>
                                        <button onClick={() => handleIncreaseQuantity(item.ticketId, true)}>+</button>
                                    </div>
                                    <span>${(item.ticket.price * item.quantity).toFixed(2)}</span>
                                    <button onClick={() => handleRemove(item.ticketId, item.ticket.movieId)}>Remove</button>
                                </li>
                            ))}
                        </ul>
                        <div className="cart-total">
                            <div className="cart-total-line">
                                <strong>Subtotal:</strong> <span>${total.toFixed(2)}</span>
                            </div>
                            {user.age >= 65 && (
                                <div className="cart-total-line">
                                    <strong>Senior Discount:</strong> <span>-${discount.toFixed(2)}</span>
                                </div>
                            )}
                            <div className="cart-total-line">
                                <strong>Tax:</strong> <span>${tax.toFixed(2)}</span>
                            </div>
                            <div className="cart-total-line">
                                <strong>Total:</strong> <span>${totalAfterTax.toFixed(2)}</span>
                            </div>
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
