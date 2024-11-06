// components/Payment.jsx

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Payment.css';

const Payment = ({ globalState }) => {
    const [cardNumber, setCardNumber] = useState('');
    const [expiryDate, setExpiryDate] = useState('');
    const [cvv, setCvv] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const { user, cart, setCart } = globalState;

    const handlePayment = async (e) => {
        e.preventDefault();

        // Prepare payment data
        const paymentData = {
            cartId: cart.id,
            cardNumber,
            exp: expiryDate,
            cardholderName: user.name,
            cvc: cvv
        };

        try {
            const response = await fetch('/movie/processpayment', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(paymentData)
            });

            if (response.ok) {
                alert('Payment processed successfully!');
                setCart(null);
                navigate('/home'); // Redirect to home page on successful payment
            } else {
                const errorData = await response.text();
                setError(errorData || 'Payment failed.');
            }
        } catch (error) {
            setError(`An error occurred while processing the payment. ${error}`);
        }
    };

    const handleGoHome = () => {
        navigate('/home');
    };

    return (
        <div className="screen">
            <div className="payment-container">
                <h1>Payment Page</h1>
                <form onSubmit={handlePayment} className="payment-form">
                    {error && <p className="error-message">{error}</p>}

                    <label htmlFor="cardNumber">Card Number</label>
                    <input
                        id="cardNumber"
                        type="text"
                        value={cardNumber}
                        onChange={(e) => setCardNumber(e.target.value)}
                        placeholder="1234 5678 9012 3456"
                        required
                    />

                    <label htmlFor="expiryDate">Expiration Date</label>
                    <input
                        id="expiryDate"
                        type="text"
                        value={expiryDate}
                        onChange={(e) => setExpiryDate(e.target.value)}
                        placeholder="MM/YYYY"
                        required
                    />

                    <label htmlFor="cvv">CVV</label>
                    <input
                        id="cvv"
                        type="password"
                        value={cvv}
                        onChange={(e) => setCvv(e.target.value)}
                        placeholder="123"
                        required
                    />

                    <button type="submit">Pay Now</button>
                    <button type="button" onClick={handleGoHome}>Go to Home</button>
                </form>
            </div>
        </div>
    );
};

export default Payment;
