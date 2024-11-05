/* eslint-env jest */
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import UserSettings from './UserSettings';

const mockSetUser = jest.fn();
const mockNavigate = jest.fn();

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('UserSettings Component', () => {
    const user = {
        id: 1,
        name: 'Test User',
        email: 'test@example.com',
        phoneNumber: '1234567890',
        preference: 'EMAIL',
    };

    beforeEach(() => {
        render(
            <Router>
                <UserSettings globalState={{ user, setUser: mockSetUser }} />
            </Router>
        );
    });

    test('renders user settings form', () => {
        expect(screen.getByRole('heading', { name: /user preferences/i })).toBeInTheDocument();
        expect(screen.getByLabelText('Name')).toBeInTheDocument();
        expect(screen.getByLabelText('Email')).toBeInTheDocument();
        expect(screen.getByLabelText('Phone Number')).toBeInTheDocument();
        expect(screen.getByLabelText('Contact Preference')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /update preferences/i })).toBeInTheDocument();
    });

    test('shows error message on failed update', async () => {
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: false,
            })
        );

        fireEvent.change(screen.getByLabelText('Name'), { target: { value: 'Updated User' } });
        fireEvent.change(screen.getByLabelText('Email'), { target: { value: 'updated@example.com' } });
        fireEvent.change(screen.getByLabelText('Phone Number'), { target: { value: '0987654321' } });
        fireEvent.change(screen.getByLabelText('Contact Preference'), { target: { value: 'PHONE' } });
        fireEvent.click(screen.getByRole('button', { name: /update preferences/i }));

        const errorMessage = await screen.findByText('Failed to update user preferences. Please try again.');
        expect(errorMessage).toBeInTheDocument();
    });

    test('navigates to home on successful update', async () => {
        const updatedUser = {
            id: 1,
            name: 'Updated User',
            email: 'updated@example.com',
            phoneNumber: '0987654321',
            preference: 'PHONE',
        };
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve(updatedUser),
            })
        );

        fireEvent.change(screen.getByLabelText('Name'), { target: { value: 'Updated User' } });
        fireEvent.change(screen.getByLabelText('Email'), { target: { value: 'updated@example.com' } });
        fireEvent.change(screen.getByLabelText('Phone Number'), { target: { value: '0987654321' } });
        fireEvent.change(screen.getByLabelText('Contact Preference'), { target: { value: 'PHONE' } });
        fireEvent.click(screen.getByRole('button', { name: /update preferences/i }));

        await screen.findByRole('button', { name: /update preferences/i }); // Wait for the update process to complete

        expect(mockSetUser).toHaveBeenCalledWith(updatedUser);
        expect(mockNavigate).toHaveBeenCalledWith('/home');
    });
});
