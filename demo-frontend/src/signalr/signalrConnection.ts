import * as signalR from '@microsoft/signalr';

let connection: signalR.HubConnection | null = null;
let heartbeatInterval: NodeJS.Timeout | null = null;

const startHeartbeat = (conn: signalR.HubConnection) => {
    stopHeartbeat();
    heartbeatInterval = setInterval(() => {
        conn.invoke("Heartbeat")
            .catch(err => console.error("Heartbeat failed:", err.message));
    }, 30000);
};

const stopHeartbeat = () => {
    if (heartbeatInterval) {
        clearInterval(heartbeatInterval);
        heartbeatInterval = null;
    }
};

export const getSignalRConnection = async (): Promise<signalR.HubConnection> => {
    if (connection) return connection;
    const token = localStorage.getItem('token') || '';
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${process.env.REACT_APP_URL_PRESENCE}?access_token=${token}`, {
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    await connection.start();
    console.log('✅ SignalR Connected');

    startHeartbeat(connection); // Start heartbeat after connection

    return connection;
};

export const stopSignalRConnection = async () => {
    if (connection) {
        await connection.stop();
        connection = null;
        console.log('❌ SignalR Disconnected');
    }
    stopHeartbeat(); // Stop heartbeat interval when disconnect
};
