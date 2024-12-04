import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import SignUp from './SignUp';

const mockSetUser = jest.fn();
const mockNavigate = jest.fn();

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('SignUp Component', () => {
    beforeEach(() => {
        render(
            <Router>
                <SignUp globalState={{ setUser: mockSetUser }} />
            </Router>
        );
    });

    test('renders sign up form', () => {
        expect(screen.getByRole('heading', { name: /sign up/i })).toBeInTheDocument();
        expect(screen.getByLabelText('Username')).toBeInTheDocument();
        expect(screen.getByLabelText('Password')).toBeInTheDocument();
        expect(screen.getByLabelText('Confirm Password')).toBeInTheDocument();
        expect(screen.getByLabelText('Name')).toBeInTheDocument();
        expect(screen.getByLabelText('Gender')).toBeInTheDocument();
        expect(screen.getByLabelText('Age')).toBeInTheDocument();
        expect(screen.getByLabelText('Email')).toBeInTheDocument();
        expect(screen.getByLabelText('Phone Number')).toBeInTheDocument();
        expect(screen.getByLabelText('Contact Preference')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /sign up/i })).toBeInTheDocument();
    });

    test('shows error message on failed sign up', async () => {
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: false,
            })
        );

        fireEvent.change(screen.getByLabelText('Username'), { target: { value: 'testuser' } });
        fireEvent.change(screen.getByLabelText('Password'), { target: { value: 'password' } });
        fireEvent.change(screen.getByLabelText('Confirm Password'), { target: { value: 'password' } });
        fireEvent.change(screen.getByLabelText('Name'), { target: { value: 'Test User' } });
        fireEvent.change(screen.getByLabelText('Gender'), { target: { value: '0' } });
        fireEvent.change(screen.getByLabelText('Age'), { target: { value: '25' } });
        fireEvent.change(screen.getByLabelText('Email'), { target: { value: 'test@example.com' } });
        fireEvent.change(screen.getByLabelText('Phone Number'), { target: { value: '1234567890' } });
        fireEvent.change(screen.getByLabelText('Contact Preference'), { target: { value: '0' } });
        fireEvent.click(screen.getByRole('button', { name: /sign up/i }));

        const errorMessage = await screen.findByText('Failed to sign up. Please try again.');
        expect(errorMessage).toBeInTheDocument();
    });

    test('navigates to home on successful sign up', async () => {
        const mockUser = { id: 1, username: 'testuser' };
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve(mockUser),
            })
        );

        fireEvent.change(screen.getByLabelText('Username'), { target: { value: 'testuser' } });
        fireEvent.change(screen.getByLabelText('Password'), { target: { value: 'password' } });
        fireEvent.change(screen.getByLabelText('Confirm Password'), { target: { value: 'password' } });
        fireEvent.change(screen.getByLabelText('Name'), { target: { value: 'Test User' } });
        fireEvent.change(screen.getByLabelText('Gender'), { target: { value: '0' } });
        fireEvent.change(screen.getByLabelText('Age'), { target: { value: '25' } });
        fireEvent.change(screen.getByLabelText('Email'), { target: { value: 'test@example.com' } });
        fireEvent.change(screen.getByLabelText('Phone Number'), { target: { value: '1234567890' } });
        fireEvent.change(screen.getByLabelText('Contact Preference'), { target: { value: '0' } });
        fireEvent.click(screen.getByRole('button', { name: /sign up/i }));

        await screen.findByRole('button', { name: /sign up/i }); // Wait for the sign up process to complete

        expect(mockSetUser).toHaveBeenCalledWith(mockUser);
        expect(mockNavigate).toHaveBeenCalledWith('/home');
    });
});
