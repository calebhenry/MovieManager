import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './UserSettings.css';

const UserSettings = ({ globalState }) => {
    const { user, setUser } = globalState;

    // State Variables
    const [name, setName] = useState(user.name);
    const [email, setEmail] = useState(user.email);
    const [phoneNumber, setPhoneNumber] = useState(user.phoneNumber);
    const [preference, setPreference] = useState(user.preference);
    const [error, setError] = useState('');

    const navigate = useNavigate();

    // Handles the process of updating user information by sending it to the API
    const handleUpdate = async (e) => {
        e.preventDefault();

        // Create an updated user object with new data
        const updatedUser = {
            id: user.id,
            name,
            email,
            phoneNumber,
            preference: Number(preference)
        };

        try {
            // Send updated user data to the API using a PUT request
            const response = await fetch('movie/updateuser/', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updatedUser),
            }); // Update the user in the API

            if (response.ok) {
                // Update the user and redirect to home page on successful update
                user.name = updatedUser.name;
                user.email = updatedUser.email;
                user.phoneNumber = updatedUser.phoneNumber;
                user.preference = updatedUser.preference;
                setUser(user);
                navigate('/home');
                alert("Successfully updated user preferences!");
            } else {
                setError('Failed to update user preferences. Please try again.');
            }
        } catch (error) {
            setError('An error occurred. Please try again.');
        }
    };

    // Logs the user out by clearing the user data from global state and redirecting to the login page
    const logout = () => {
        setUser(null);
        navigate("/");
    }

    return (
        <div className="screen">
            <div className="preferences-container">
                <h1>User Preferences</h1>
                {/* Form to update user preferences */}
                <form onSubmit={handleUpdate} className="preferences-form">
                    {error && <p className="error-message">{error}</p>}

                    <label htmlFor="name">Name</label>
                    <input
                        id="name"
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter your name"
                    />

                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                    />

                    <label htmlFor="phoneNumber">Phone Number</label>
                    <input
                        id="phoneNumber"
                        type="tel"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                        placeholder="Enter your phone number"
                    />

                    <label htmlFor="preference">Contact Preference</label>
                    <select id="preference" value={preference} onChange={(e) => setPreference(e.target.value)}>
                        <option value="1">Email</option>
                        <option value="2">Phone</option>
                    </select>

                    <button type="submit">Update Preferences</button>
                    <button onClick={logout}>Logout</button>
                </form>
            </div>
        </div>
    );
};

export default UserSettings;
