import { useEffect } from 'react';
import { subscribeEvent, unsubscribeEvent } from 'signalr/signalrEventEmitter';

export const useSignalREvent = (eventName: string, handler: (...args: any[]) => void) => {
    useEffect(() => {
        subscribeEvent(eventName, handler);

        return () => {
            unsubscribeEvent(eventName, handler);
        };
    }, [eventName, handler]);
};
