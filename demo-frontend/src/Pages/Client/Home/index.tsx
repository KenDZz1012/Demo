import React from 'react';
import { useNavigate } from 'react-router-dom';

const Home: React.FC = () => {
    const navigate = useNavigate();

    return (
        <div
            style={{
                backgroundColor: '#5865F2', // Discord primary color
                height: '100vh',
                color: 'white',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                textAlign: 'center',
            }}
        >
            <h1 style={{ fontSize: '3rem', marginBottom: '1.5rem' }}>
                Chào mừng đến với KenVerse
            </h1>
            <button
                style={{
                    padding: '0.75rem 1.5rem',
                    fontSize: '1rem',
                    backgroundColor: 'white',
                    color: '#5865F2',
                    border: 'none',
                    borderRadius: '9999px',
                    cursor: 'pointer',
                }}
                onClick={() => navigate('/login')}
            >
                Đăng nhập
            </button>
        </div>
    );
};

export default Home;
