import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './SignUp.css';

const SignUp = ({ globalState }) => {

    // State Variables
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [name, setName] = useState('');
    const [gender, setGender] = useState(0);
    const [age, setAge] = useState('');
    const [email, setEmail] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [preference, setPreference] = useState(0);
    const [permissionLevel, setPermissionLevel] = useState(0);
    const [error, setError] = useState('');

    const { user, setUser } = globalState;
    const navigate = useNavigate();

    // Handles the sign-up process by validating the form and submitting the user information to the server
    const handleSignUp = async (e) => {
        e.preventDefault();

        // Check if the passwords match
        if (password !== confirmPassword) {
            setError('Passwords do not match.');
            return;
        }

        // Create a user object with all the input data
        const user = {
            username,
            password,
            name,
            gender,
            age: parseInt(age),
            email,
            phoneNumber,
            preference,
            permissionLevel
        };

        try {
            // Send the user data to the API to create a new account
            const response = await fetch('movie/adduser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(user),
            }); // Get the new user from the API

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
                {/* Sign-up form */}
                <form onSubmit={handleSignUp} className="signup-form">
                    {error && <p className="error-message">{error}</p>}

                    <label htmlFor="username">Username</label>
                    <input
                        id="username"
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        placeholder="Enter your username"
                        required
                    />

                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your password"
                        required
                    />

                    <label htmlFor="confirmPassword">Confirm Password</label>
                    <input
                        id="confirmPassword"
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        placeholder="Confirm your password"
                        required
                    />

                    <label htmlFor="name">Name</label>
                    <input
                        id="name"
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter your name"
                        required
                    />

                    <label htmlFor="gender">Gender</label>
                    <select id="gender" value={gender} onChange={(e) => setGender(parseInt(e.target.value))} required>
                        <option value={0}>Male</option>
                        <option value={1}>Female</option>
                        <option value={2}>Other</option>
                    </select>

                    <label htmlFor="age">Age</label>
                    <input
                        id="age"
                        type="number"
                        value={age}
                        onChange={(e) => setAge(e.target.value)}
                        placeholder="Enter your age"
                        required
                    />

                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                        required
                    />

                    <label htmlFor="phoneNumber">Phone Number</label>
                    <input
                        id="phoneNumber"
                        type="tel"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                        placeholder="Enter your phone number"
                        required
                    />

                    <label htmlFor="preference">Contact Preference</label>
                    <select id="preference" value={preference} onChange={(e) => setPreference(parseInt(e.target.value))} required>
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
