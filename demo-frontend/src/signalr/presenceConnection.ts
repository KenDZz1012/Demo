import * as signalR from '@microsoft/signalr';

const baseUrl = process.env.REACT_APP_URL_PRESENCE;

export const createPresenceConnection = async (token?: string): Promise<signalR.HubConnection> => {
    const realToken = token || localStorage.getItem("token") || "";
    console.log("🟡 Token sẽ gửi tới SignalR:", realToken); // 🔥

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}?access_token=${realToken}`, {
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        })

        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();


    await connection.start().then(() => console.log('SignalR connected to PresenceHub'))
        .catch(err => console.error('SignalR connection error:', err));;
    console.log('SignalR connected to PresenceHub');

    return connection;
};
