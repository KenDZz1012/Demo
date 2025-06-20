import * as signalR from '@microsoft/signalr';

const baseUrl = process.env.REACT_APP_URL_PRESENCE;

export const createPresenceConnection = async (): Promise<signalR.HubConnection> => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}/presence`, {
            accessTokenFactory: () => localStorage.getItem("token") || "", // token được truyền vào header Authorization
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

    await connection.start();
    console.log('SignalR connected to PresenceHub');

    return connection;
};
