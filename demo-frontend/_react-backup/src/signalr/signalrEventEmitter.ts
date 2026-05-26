import { EventEmitter } from 'events';

export const signalrEmitter = new EventEmitter();
const registeredEvents = new Set<string>();

export const registerSignalREvent = (connection: signalR.HubConnection, eventName: string) => {
    if (registeredEvents.has(eventName)) return;
    connection.on(eventName, (...args) => {
        signalrEmitter.emit(eventName, ...args);
    });
    registeredEvents.add(eventName);
};

export const subscribeEvent = (eventName: string, callback: (...args: any[]) => void) => {
    signalrEmitter.on(eventName, callback);
};

export const unsubscribeEvent = (eventName: string, callback: (...args: any[]) => void) => {
    signalrEmitter.off(eventName, callback);
};
