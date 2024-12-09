import { useEffect, useState } from 'react';
import React from 'react';

const MovieReply = ({username, message}) => {
    return (
        <>
            <p className="left">{username}: {message}</p>
        </>
    );
};

export default MovieReply ;
