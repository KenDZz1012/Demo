import * as signalR from '@microsoft/signalr';

const baseUrl = process.env.REACT_APP_URL_PRESENCE;
let connection: signalR.HubConnection | null = null;
let heartbeatInterval: NodeJS.Timeout;

export const createPresenceConnection = async (token?: string): Promise<signalR.HubConnection> => {
    if (connection) return connection; 

    const realToken = token || localStorage.getItem("token") || "";
    const url = `${baseUrl}?access_token=${realToken}`;
    connection = new signalR.HubConnectionBuilder()
        .withUrl(url, {
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    try {
        await connection.start();
        startHeartbeat(connection);
    } catch (err) {
        console.error('❌ SignalR connection error:', err);
    }

    return connection;
};

const startHeartbeat = (conn: signalR.HubConnection) => {
    heartbeatInterval = setInterval(() => {
        conn.invoke("Heartbeat")
            .catch(err => console.error("❌ Heartbeat failed:", err.message));
    }, 30000);
};

export const stopPresenceConnection = async () => {
    if (connection) {
        clearInterval(heartbeatInterval);
        await connection.stop();
        connection = null;
    }
};

export const onEvent = <T>(eventName: string, callback: (data: T) => void) => {
    if (!connection) return;
    connection.on(eventName, callback);
};

export const offEvent = (eventName: string) => {
    if (!connection) return;
    connection.off(eventName);
};
