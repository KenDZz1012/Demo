import React, { useEffect, useState } from 'react';

const LoadingScreen = () => {
    return (
        <div className="loading-container">
            <img src="/logo.png" alt="Logo" className="rotating-logo" />
        </div>
    );
};

export default LoadingScreen;
