import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import '@testing-library/jest-dom';
import Payment from './Payment';
import { BrowserRouter as Router } from 'react-router-dom';

// Mock fetch
global.fetch = jest.fn();

const mockGlobalState = {
    user: { name: 'Test User' },
    cart: { id: 1 },
    setCart: jest.fn(),
};

describe('Payment Component', () => {
    beforeEach(() => {
        fetch.mockClear();
    });

    test('renders payment form', () => {
        render(
            <Router>
                <Payment globalState={mockGlobalState} />
            </Router>
        );

        expect(screen.getByText('Payment Page')).toBeInTheDocument();
        expect(screen.getByLabelText('Cardholder Name')).toBeInTheDocument();
        expect(screen.getByLabelText('Card Number')).toBeInTheDocument();
        expect(screen.getByLabelText('Expiration Date')).toBeInTheDocument();
        expect(screen.getByLabelText('CVV')).toBeInTheDocument();
        expect(screen.getByText('Pay Now')).toBeInTheDocument();
        expect(screen.getByText('Go to Home')).toBeInTheDocument();
    });

    test('handles successful payment', async () => {
        fetch.mockImplementationOnce(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve({}),
            })
        );

        render(
            <Router>
                <Payment globalState={mockGlobalState} />
            </Router>
        );

        fireEvent.change(screen.getByLabelText('Cardholder Name'), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByLabelText('Card Number'), { target: { value: '1234 5678 9012 3456' } });
        fireEvent.change(screen.getByLabelText('Expiration Date'), { target: { value: '12/2025' } });
        fireEvent.change(screen.getByLabelText('CVV'), { target: { value: '123' } });

        await act(async () => {
            fireEvent.click(screen.getByText('Pay Now'));
        });

        await waitFor(() => {
            expect(mockGlobalState.setCart).toHaveBeenCalledWith(null);
            expect(window.location.pathname).toBe('/home');
        });
    });

    test('handles payment failure', async () => {
        fetch.mockImplementationOnce(() =>
            Promise.resolve({
                ok: false,
                text: () => Promise.resolve('Payment failed.'),
            })
        );

        render(
            <Router>
                <Payment globalState={mockGlobalState} />
            </Router>
        );

        fireEvent.change(screen.getByLabelText('Cardholder Name'), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByLabelText('Card Number'), { target: { value: '1234 5678 9012 3456' } });
        fireEvent.change(screen.getByLabelText('Expiration Date'), { target: { value: '12/2025' } });
        fireEvent.change(screen.getByLabelText('CVV'), { target: { value: '123' } });

        await act(async () => {
            fireEvent.click(screen.getByText('Pay Now'));
        });

        await waitFor(() => {
            expect(screen.getByText('Payment failed.')).toBeInTheDocument();
        });
    });

    test('handles go to home', () => {
        render(
            <Router>
                <Payment globalState={mockGlobalState} />
            </Router>
        );

        fireEvent.click(screen.getByText('Go to Home'));

        expect(window.location.pathname).toBe('/home');
    });
});
