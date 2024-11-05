import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

const Login = ({ globalState }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const { setUser } = globalState;

    const handleLogin = async (e) => {
        e.preventDefault();
        const response = await fetch(`movie/getuser?username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        }); // Get the user from the API

        try {
            if (response.ok) {
                const user = await response.json();
                if (user) {
                    // Redirect to home page on successful login
                    setUser(user);
                    navigate('/home');
                } else {
                    setError('Invalid username or password.'); // If the user is null, the username or password is incorrect
                }
            } else {
                setError('Failed to login. Please try again.');
            }
        } catch (error) {
            setError('An error occurred. Please try again.');
        }
    };

    const handleSignUp = () => {
        navigate('/signup');
    };

    return (
        <div className="screen">
            <div className="login-container">
                <h1>Login</h1>
                <form onSubmit={handleLogin} className="login-form">
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

                    <button type="submit">Login</button>
                    <button type="button" onClick={handleSignUp}>Sign Up</button>
                </form>
            </div>
        </div>
    );
};

export default Login;
