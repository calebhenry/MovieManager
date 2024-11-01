import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './components/Home';
import MovieListing from './components/MovieListing';
import Payment from './components/Payment';
import Cart from './components/Cart';
import './App.css';

function App() {
    return (
        <div className="App">
            <Router>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/payment" element={<Payment />} />
                    <Route path="/cart" element={<Cart />} />
                </Routes>
            </Router>
        </div>
    );
}

export default App;