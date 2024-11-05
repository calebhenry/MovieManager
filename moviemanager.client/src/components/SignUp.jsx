import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './SignUp.css';

const SignUp = ({ globalState }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [name, setName] = useState('');
    const [gender, setGender] = useState(0);
    const [age, setAge] = useState('');
    const [email, setEmail] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [preference, setPreference] = useState(0);
    const [error, setError] = useState('');
    const { user, setUser } = globalState;
    const navigate = useNavigate();

    const handleSignUp = async (e) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            setError('Passwords do not match.');
            return;
        }

        const user = {
            username,
            password,
            name,
            gender,
            age: parseInt(age),
            email,
            phoneNumber,
            preference
        };

        try {
            const response = await fetch('movie/adduser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(user),
            });

            console.dir(response);

            if (response.ok) {
                const user = await response.json();
                if (user) {
                    // Redirect to home page on successful sign up
                    setUser(user);
                    navigate('/home');
                }
            } else {
                setError('Failed to sign up. Please try again.');
            }
        } catch (error) {
            setError('An error occurred. Please try again.');
        }
    };

    return (
        <div className="screen">
            <div className="signup-container">
                <h1>Sign Up</h1>
                <form onSubmit={handleSignUp} className="signup-form">
                    {error && <p className="error-message">{error}</p>}

                    <label>Username</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        placeholder="Enter your username"
                        required
                    />

                    <label>Password</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your password"
                        required
                    />

                    <label>Confirm Password</label>
                    <input
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        placeholder="Confirm your password"
                        required
                    />

                    <label>Name</label>
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter your name"
                        required
                    />

                    <label>Gender</label>
                    <select value={gender} onChange={(e) => setGender(parseInt(e.target.value))} required>
                        <option value={0}>Male</option>
                        <option value={1}>Female</option>
                        <option value={2}>Other</option>
                    </select>

                    <label>Age</label>
                    <input
                        type="number"
                        value={age}
                        onChange={(e) => setAge(e.target.value)}
                        placeholder="Enter your age"
                        required
                    />

                    <label>Email</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                        required
                    />

                    <label>Phone Number</label>
                    <input
                        type="tel"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                        placeholder="Enter your phone number"
                        required
                    />

                    <label>Contact Preference</label>
                    <select value={preference} onChange={(e) => setPreference(parseInt(e.target.value))} required>
                        <option value={0}>Email</option>
                        <option value={1}>Phone</option>
                    </select>

                    <button type="submit">Sign Up</button>
                </form>
            </div>
        </div>
    );
};

export default SignUp;
