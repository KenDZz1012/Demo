import { EventEmitter } from 'events';
import { getSignalRConnection } from './signalrConnection';

const signalrEmitter = new EventEmitter();
const registeredEvents = new Set<string>();

export const initializeSignalREvents = async () => {
    const connection = await getSignalRConnection();

    const bindEvent = (eventName: string) => {
        if (registeredEvents.has(eventName)) return;
        connection.on(eventName, (...args) => {
            signalrEmitter.emit(eventName, ...args);
        });
        registeredEvents.add(eventName);
    };

    signalrEmitter.on('subscribe', bindEvent);
};

export const subscribeEvent = (eventName: string, callback: (...args: any[]) => void) => {
    signalrEmitter.emit('subscribe', eventName);
    signalrEmitter.on(eventName, callback);
};

export const unsubscribeEvent = (eventName: string, callback: (...args: any[]) => void) => {
    signalrEmitter.off(eventName, callback);
};
