import React, { useState } from 'react';
import {
    BrowserRouter as Router,
    Route, 
    Routes,
    Navigate
} from "react-router-dom";
import Home from './components/Home';
import MovieListing from './components/MovieListing';
import Payment from './components/Payment';
import Cart from './components/Cart';
import Login from './components/Login';
import SignUp from './components/SignUp';
import UserSettings from './components/UserSettings';
import Manager from './components/Manager';

import './App.css';

function App() {
    const [user, setUser] = useState(null);
    const [cart, setCart] = useState(null);
    const [movies, setMovies] = useState([]);

    const globalState = {
        user, setUser,
        cart, setCart,
        movies, setMovies
    }; // Holds the global state

    return (
        <div className="App">
            <Router>
                <Routes>
                    <Route path="/" element={<Navigate to="/login" />} />
                    <Route path="/login" element={<Login globalState={globalState} />} />
                    <Route path="/signup" element={<SignUp globalState={globalState} />} />
                    <Route path="/movies/:id" element={user == null ? <Navigate to="/login" /> : <MovieListing globalState={globalState} />} />
                    <Route path="/payment" element={user == null ? <Navigate to="/login" /> : <Payment globalState={globalState} />} />
                    <Route path="/cart" element={user == null ? <Navigate to="/login" /> : <Cart globalState={globalState} />} />
                    <Route path="/home" element={user == null ? <Navigate to="/login" /> : <Home globalState={globalState} />} />
                    <Route path="/settings" element={user == null ? <Navigate to="/login" /> : <UserSettings globalState={globalState} />} />
                    <Route path="/manager" element={user == null ? <Navigate to="/login" /> : <Manager globalState={globalState} /> } />
                </Routes>
            </Router>
        </div>
    );
}

export default App;