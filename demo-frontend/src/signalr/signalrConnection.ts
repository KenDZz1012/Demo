import * as signalR from '@microsoft/signalr';
import { registerSignalREvent } from './signalrEventEmitter';

let connection: signalR.HubConnection | null = null;
let heartbeatInterval: NodeJS.Timeout | null = null;

const startHeartbeat = (conn: signalR.HubConnection) => {
    stopHeartbeat();
    heartbeatInterval = setInterval(() => {
        conn.invoke("Heartbeat").catch(err => console.error("Heartbeat failed:", err.message));
    }, 30000);
};

const stopHeartbeat = () => {
    if (heartbeatInterval) {
        clearInterval(heartbeatInterval);
        heartbeatInterval = null;
    }
};

export const startSignalRConnection = async () => {
    if (connection) return;

    const token = localStorage.getItem('token');
    if (!token) {
        console.error('Token is missing. Cannot start SignalR connection.');
        return;
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${process.env.REACT_APP_URL_PRESENCE}?access_token=${token}`, {
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    await connection.start();
    console.log('✅ SignalR Connected');
    startHeartbeat(connection);

    // Register Events after connection established
    registerSignalREvent(connection, 'friendRequestReceived');
    registerSignalREvent(connection, 'friendRequestAccepted');
    registerSignalREvent(connection, 'friendRequestRejected');
};

export const stopSignalRConnection = async () => {
    if (connection) {
        await connection.stop();
        connection = null;
        console.log('❌ SignalR Disconnected');
    }
    stopHeartbeat();
};
