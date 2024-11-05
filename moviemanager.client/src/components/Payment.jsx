// components/Payment.jsx

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Payment.css';
import { Link } from 'react-router-dom';

const Payment = () => {
    const [cardNumber, setCardNumber] = useState('');
    const [expiryDate, setExpiryDate] = useState('');
    const [cvv, setCvv] = useState('');
    const [cardholderName, setCardholderName] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const validateCardNumber = (number) => /^\d{16}$/.test(number);
    const validateExpiryDate = (date) => /^(0[1-9]|1[0-2])\/\d{2}$/.test(date);
    const validateCVV = (cvv) => /^\d{3}$/.test(cvv);
    const validateCardholderName = (name) => typeof name === 'string' && name.trim() !== '';

    const handlePayment = (e) => {
        e.preventDefault();
        
        // Validate inputs
        if (!validateCardNumber(cardNumber)) {
            setError('Invalid card number. Must be 16 digits.');
            return;
        }
        if (!validateCardholderName(cardholderName)) {
            setError('Invalid name. Please fill out field.');
            return;
        }
        if (!validateExpiryDate(expiryDate)) {
            setError('Invalid expiry date. Use MM/YY format.');
            return;
        }
        if (!validateCVV(cvv)) {
            setError('Invalid CVV. Must be 3 digits.');
            return;
        }

        // Clear error if all validations pass
        setError('');

        // Simulate successful payment processing
        alert('Payment processed successfully!');
        navigate('/'); // Redirect to home page on successful payment
    };

    return (
        <div className="payment-container">
            <h1>Proceed to Payment</h1>
            <div className="payment-nav">
                <Link to="/">Keep Browsing Movies</Link> <br></br>
                <Link to="/cart">Go to Cart</Link>
            </div>
            <form onSubmit={handlePayment} className="payment-form">
                {error && <p className="error-message">{error}</p>}

                <label>Card Number</label>
                <input
                    type="text"
                    value={cardNumber}
                    onChange={(e) => setCardNumber(e.target.value)}
                    placeholder="1234 5678 9012 3456"
                    required
                />

                <label>Cardholder Name</label>
                <input
                    type="text"
                    value={cardholderName}
                    onChange={(e) => setCardholderName(e.target.value)}
                    placeholder="John Doe"
                    required
                />

                <label>Expiration Date (MM/YY)</label>
                <input
                    type="text"
                    value={expiryDate}
                    onChange={(e) => setExpiryDate(e.target.value)}
                    placeholder="MM/YY"
                    required
                />

                <label>CVV</label>
                <input
                    type="password"
                    value={cvv}
                    onChange={(e) => setCvv(e.target.value)}
                    placeholder="123"
                    required
                />

                <button type="submit">Pay Now</button>
            </form>
        </div>
    );
};

export default Payment;
