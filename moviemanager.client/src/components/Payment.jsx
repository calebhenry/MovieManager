import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Payment.css';

const Payment = ({ globalState }) => {
    const [streetAddress, setStreetAddress] = useState('');
    const [city, setCity] = useState('');
    const [state, setState] = useState('');
    const [zipCode, setZipCode] = useState('');
    const [cardNumber, setCardNumber] = useState('');
    const [expiryDate, setExpiryDate] = useState('');
    const [cardName, setCardName] = useState('');
    const [cvv, setCvv] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const { user, cart, setCart } = globalState;

    const handlePayment = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch(`/movie/processpayment?cartId=${cart.id}&streetAddress=${streetAddress}&city=${city}&state=${state}&zipCode=${zipCode}&cardNumber=${cardNumber}&exp=${expiryDate}&cardholderName=${cardName}&cvc=${cvv}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                if(user.preference == 1) {
                    alert(`Payment was processed successfully! Email confirmation was sent to ${user.email}.`);
                } else {
                    alert(`Payment was processed successfully! Text confirmation was sent to ${user.phoneNumber}.`)
                }
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
                    
                    <h2>Billing Information</h2>
                    <label htmlFor="streetAddress">Street Address</label>
                    <input
                        id="streetAddress"
                        type="text"
                        value={streetAddress}
                        onChange={(e) => setStreetAddress(e.target.value)}
                        placeholder="123 Main St"
                        required
                    />

                    <div className="city-state-wrapper">
                        <label htmlFor="city">City</label>
                        <input
                            id="city"
                            type="text"
                            value={city}
                            onChange={(e) => setCity(e.target.value)}
                            placeholder="Columbia"
                            required
                        />

                        <label htmlFor="state">State</label>
                        <input
                            id="state"
                            type="text"
                            maxLength="2"
                            value={state}
                            onChange={(e) => setState(e.target.value)}
                            placeholder="SC"
                            required
                        />
                    </div>

                    <label htmlFor="zipCode">Zip Code</label>
                    <input
                        id="zipCode"
                        type="text"
                        maxLength="5"
                        value={zipCode}
                        onChange={(e) => setZipCode(e.target.value)}
                        placeholder="12345"
                        required
                    />

                    <h2>Card Information</h2>
                    <label htmlFor="cardName">Cardholder Name</label>
                    <input
                        id="cardName"
                        type="text"
                        value={cardName}
                        onChange={(e) => setCardName(e.target.value)}
                        placeholder="John Doe"
                        required
                    />

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

                    {error && <p className="error-message">{error}</p>}

                    <button type="submit">Pay Now</button>
                    <button type="button" onClick={handleGoHome}>Go to Home</button>
                </form>
            </div>
        </div>
    );
};

export default Payment;
