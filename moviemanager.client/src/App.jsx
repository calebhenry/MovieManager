import React from 'react';
import {
    BrowserRouter as Router,
    Route, 
    Routes,
    Navigate
} from "react-router-dom";
import Home from './components/Home';
import Login from './components/Login';
import Signup from './components/Signup';
import UserSettings from './components/UserSettings';
import './App.css';

function App() {
    const [user, setUser] = React.useState(null);

    const globalState = {
        user, setUser
    }; // Holds the global state

    return (
        <div className="App">
            <Router>
                <Routes>
                    <Route path="/" element={<Navigate to="/login" />} />
                    <Route path="/login" element={<Login globalState={globalState} />} />
                    <Route path="/signup" element={<Signup globalState={globalState} />} />
                    <Route path="/home" element={user == null ? <Navigate to="/login" /> : <Home globalState={globalState} />} />
                    <Route path="/settings" element={user == null ? <Navigate to="/login" /> : <UserSettings globalState={globalState} />} />
                </Routes>
            </Router>
        </div>
    );
}

export default App;