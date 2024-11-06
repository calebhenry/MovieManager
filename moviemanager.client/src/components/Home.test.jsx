import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import Home from './Home';
import { BrowserRouter as Router } from 'react-router-dom';

// Mock fetch
global.fetch = jest.fn(() =>
    Promise.resolve({
        json: () => Promise.resolve([]), // Mock empty movies array
    })
);

const mockGlobalState = {
    movies: [],
    setMovies: jest.fn(),
    user: { name: 'Test User' },
    cart: null,
    setCart: jest.fn(),
};

describe('Home Component', () => {
    beforeEach(() => {
        fetch.mockClear();
    });

    test('renders loading state initially', () => {
        render(
            <Router>
                <Home globalState={mockGlobalState} />
            </Router>
        );
        expect(screen.getByText('Loading movies...')).toBeInTheDocument();
    });

    test('renders welcome message with user name', async () => {
        render(
            <Router>
                <Home globalState={mockGlobalState} />
            </Router>
        );

        await waitFor(() => expect(screen.getByText('Welcome to Movie Browser Test User!')).toBeInTheDocument());
    });

    test('renders no movies available message', async () => {
        render(
            <Router>
                <Home globalState={mockGlobalState} />
            </Router>
        );

        await waitFor(() => expect(screen.getByText('No movies available')).toBeInTheDocument());
    });

    test('renders movies when available', async () => {
        const movies = [
            { id: 1, name: 'Movie 1', description: 'Description 1' },
            { id: 2, name: 'Movie 2', description: 'Description 2' },
        ];

        fetch.mockImplementationOnce(() =>
            Promise.resolve({
                json: () => Promise.resolve(movies),
            })
        );

        render(
            <Router>
                <Home globalState={{ ...mockGlobalState, movies }} />
            </Router>
        );

        await waitFor(() => {
            expect(screen.getByText('Movie 1')).toBeInTheDocument();
            expect(screen.getByText('Movie 2')).toBeInTheDocument();
        });
    });
});
