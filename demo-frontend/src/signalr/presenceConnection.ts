import * as signalR from '@microsoft/signalr';

const baseUrl = process.env.REACT_APP_URL_PRESENCE;

export const createPresenceConnection = async (token?: string): Promise<signalR.HubConnection> => {
    const realToken = token || localStorage.getItem("token") || "";
    console.log("🟡 Token sẽ gửi tới SignalR:", realToken);

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}?access_token=${realToken}`, {
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    await connection
        .start()
        .then(() => {
            console.log('✅ SignalR connected to PresenceHub');

            // ✅ Gửi Heartbeat mỗi 30s sau khi kết nối thành công
            setInterval(() => {
                connection.invoke("Heartbeat")
                    .then(() => console.log("❤️ Heartbeat sent"))
                    .catch(err => console.error("❌ Heartbeat failed:", err));
            }, 30000); // 30s
        })
        .catch(err => {
            console.error('❌ SignalR connection error:', err);
        });

    return connection;
};
