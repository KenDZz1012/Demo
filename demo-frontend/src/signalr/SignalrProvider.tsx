import React, { useEffect } from 'react';
import { initializeSignalREvents } from './signalrEventEmitter';
import { stopSignalRConnection } from './signalrConnection';

export const SignalRProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    useEffect(() => {
        initializeSignalREvents();

        return () => {
            stopSignalRConnection();
        };
    }, []);

    return <>{children}</>;
};
