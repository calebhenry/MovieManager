import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './UserSettings.css';

const UserSettings = ({ globalState }) => {
    const { user, setUser } = globalState;
    const [name, setName] = useState(user.name);
    const [email, setEmail] = useState(user.email);
    const [phoneNumber, setPhoneNumber] = useState(user.phoneNumber);
    const [preference, setPreference] = useState(user.preference);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleUpdate = async (e) => {
        e.preventDefault();

        const updatedUser = {
            id: user.id,
            name,
            email,
            phoneNumber,
            preference
        };

        try {
            const response = await fetch('movie/updateuser', {
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
            } else {
                setError('Failed to update user preferences. Please try again.');
            }
        } catch (error) {
            setError('An error occurred. Please try again.');
        }
    };

    return (
        <div className="screen">
            <div className="preferences-container">
                <h1>User Preferences</h1>
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
                        <option value="EMAIL">Email</option>
                        <option value="PHONE">Phone</option>
                    </select>

                    <button type="submit">Update Preferences</button>
                </form>
            </div>
        </div>
    );
};

export default UserSettings;
