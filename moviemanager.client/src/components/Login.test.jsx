/* eslint-env jest */
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import Login from './Login';

const mockSetUser = jest.fn();
const mockNavigate = jest.fn();

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('Login Component', () => {
    beforeEach(() => {
        render(
            <Router>
                <Login globalState={{ setUser: mockSetUser }} />
            </Router>
        );
    });

    test('renders login form', () => {
        expect(screen.getByRole('heading', { name: /login/i })).toBeInTheDocument();
        expect(screen.getByLabelText('Username')).toBeInTheDocument();
        expect(screen.getByLabelText('Password')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /login/i })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /sign up/i })).toBeInTheDocument();
    });

    test('shows error message on failed login', async () => {
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: false,
            })
        );

        fireEvent.change(screen.getByLabelText('Username'), { target: { value: 'testuser' } });
        fireEvent.change(screen.getByLabelText('Password'), { target: { value: 'password' } });
        fireEvent.click(screen.getByRole('button', { name: /login/i }));

        const errorMessage = await screen.findByText('Failed to login. Please try again.');
        expect(errorMessage).toBeInTheDocument();
    });

    test('navigates to home on successful login', async () => {
        const mockUser = { id: 1, username: 'testuser' };
        global.fetch = jest.fn(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve(mockUser),
            })
        );

        fireEvent.change(screen.getByLabelText('Username'), { target: { value: 'testuser' } });
        fireEvent.change(screen.getByLabelText('Password'), { target: { value: 'password' } });
        fireEvent.click(screen.getByRole('button', { name: /login/i }));

        await screen.findByRole('button', { name: /login/i }); // Wait for the login process to complete

        expect(mockSetUser).toHaveBeenCalledWith(mockUser);
        expect(mockNavigate).toHaveBeenCalledWith('/home');
    });

    test('navigates to signup on sign up button click', () => {
        fireEvent.click(screen.getByRole('button', { name: /sign up/i }));
        expect(mockNavigate).toHaveBeenCalledWith('/signup');
    });
});
