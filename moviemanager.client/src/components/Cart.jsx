// components/Cart.jsx

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Cart.css';

const Cart = () => {
    const navigate = useNavigate();

    // TODO Sample cart data. Replace this with actual data fetching from a backend API.
    const [cartItems, setCartItems] = useState([
        { id: 1, name: 'Grand Budapest Hotel', price: 10.5, quantity: 1 },
        { id: 2, name: 'Mini Budapest Hotel', price: 8.75, quantity: 1 },
    ]);

    // Calculate total
    const total = cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);

    // Remove item from cart
    const handleRemove = (id) => {
        setCartItems((prevItems) => prevItems.filter((item) => item.id !== id));
    };

    // Increase ticket quantity
    const handleIncreaseQuantity = (id) => {
        setCartItems((prevItems) =>
            prevItems.map((item) =>
                item.id === id ? { ...item, quantity: item.quantity + 1 } : item
            )
        );
    };

    // Decrease ticket quantity
    const handleDecreaseQuantity = (id) => {
        setCartItems((prevItems) =>
            prevItems.map((item) =>
                item.id === id && item.quantity > 1 ? { ...item, quantity: item.quantity - 1 } : item
            )
        );
    };

    // Proceed to payment
    const handleProceedToPayment = () => {
        navigate('/payment');
    };

    const handleGoHome = () => {
        navigate('/');
    };

    return (
        <div className="cart-container">
            <button onClick={handleGoHome}>Go to Home</button>
            <h1>Your Cart</h1>
            {cartItems.length === 0 ? (
                <p>Your cart is empty.</p>
            ) : (
                <>
                    <ul className="cart-list">
                        {cartItems.map((item) => (
                            <li key={item.id} className="cart-item">
                                <span>{item.name}</span>
                                <span>${item.price.toFixed(2)}</span>
                                <div className="quantity-controls">
                                    <button onClick={() => handleDecreaseQuantity(item.id)}>-</button>
                                    <span>{item.quantity}</span>
                                    <button onClick={() => handleIncreaseQuantity(item.id)}>+</button>
                                </div>
                                <span>${(item.price * item.quantity).toFixed(2)}</span>
                                <button onClick={() => handleRemove(item.id)}>Remove</button>
                            </li>
                        ))}
                    </ul>
                    <div className="cart-total">
                        <strong>Total:</strong> ${total.toFixed(2)}
                    </div>
                    <button className="proceed-button" onClick={handleProceedToPayment}>
                        Proceed to Payment
                    </button>
                </>
            )}
        </div>
    );
};

export default Cart;
