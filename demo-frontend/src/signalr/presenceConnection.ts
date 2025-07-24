import * as signalR from '@microsoft/signalr';

const baseUrl = process.env.REACT_APP_URL_PRESENCE;


export const createPresenceConnection = async (token?: string): Promise<signalR.HubConnection> => {
    const realToken = token || localStorage.getItem("token") || "";
    const url = `${baseUrl}?access_token=${realToken}`;
    console.log("🟡 Connecting to SignalR at:", url);

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(url, {
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    try {
        await connection.start();
        console.log('✅ SignalR connected to PresenceHub');

        startHeartbeat(connection); // tách riêng
    } catch (err) {
        console.error('❌ SignalR connection error:', err);
    }

    return connection;
};

let heartbeatInterval: NodeJS.Timeout;

const startHeartbeat = (connection: signalR.HubConnection) => {
    heartbeatInterval = setInterval(() => {
        connection.invoke("Heartbeat")
            .then(() => console.log("❤️ Heartbeat sent"))
            .catch(err => console.error("❌ Heartbeat failed:", err.message));
    }, 30000);
};
