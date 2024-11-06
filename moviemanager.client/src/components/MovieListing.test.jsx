import React from 'react';
import { render, screen, waitFor, fireEvent, act } from '@testing-library/react';
import '@testing-library/jest-dom';
import MovieListing from './MovieListing';
import { BrowserRouter as Router } from 'react-router-dom';

// Mock fetch
global.fetch = jest.fn();

const mockGlobalState = {
    cart: { id: 1, tickets: [] },
    setCart: jest.fn(),
};

const mockMovie = {
    id: 1,
    name: 'Test Movie',
    description: 'Test Description',
    tickets: [
        { id: 1, showtime: '2023-10-05T14:48:00.000Z' },
        { id: 2, showtime: '2023-10-06T16:00:00.000Z' },
    ],
};

describe('MovieListing Component', () => {
    beforeEach(() => {
        fetch.mockClear();
    });

    test('renders loading state initially', () => {
        render(
            <Router>
                <MovieListing globalState={mockGlobalState} />
            </Router>
        );
        expect(screen.getByText('Loading movie details...')).toBeInTheDocument();
    });

    test('renders movie details', async () => {
        fetch.mockImplementationOnce(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve(mockMovie),
            })
        );

        render(
            <Router>
                <MovieListing globalState={mockGlobalState} />
            </Router>
        );

        await waitFor(() => {
            expect(screen.getByText('Test Movie')).toBeInTheDocument();
            expect(screen.getByText('Test Description')).toBeInTheDocument();
            expect(screen.getByText('10/05 10:48')).toBeInTheDocument();
            expect(screen.getByText('10/06 12:00')).toBeInTheDocument();
        });
    });

    test('renders error message on fetch failure', async () => {
        fetch.mockImplementationOnce(() =>
            Promise.reject(new Error('Failed to load movie details'))
        );

        render(
            <Router>
                <MovieListing globalState={mockGlobalState} />
            </Router>
        );

        await waitFor(() => {
            expect(screen.getByText('Failed to load movie details: Failed to load movie details')).toBeInTheDocument();
        });
    });

    test('handles go to home', async () => {
        fetch.mockImplementationOnce(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve(mockMovie),
            })
        );

        render(
            <Router>
                <MovieListing globalState={mockGlobalState} />
            </Router>
        );

        await waitFor(() => {
            expect(screen.getByText('Test Movie')).toBeInTheDocument();
        });

        const goHomeButton = screen.getByText('Go to Home');
        fireEvent.click(goHomeButton);

        expect(window.location.pathname).toBe('/home');
    });
});
