import { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

let connection: signalR.HubConnection;
const baseUrl = process.env.REACT_APP_URL_PRESENCE;

export const usePresenceSocket = (userId: string, onFriendRequestReceived: (fromUserId: string, fromUserName: string, fromUserDisplayName: string, fromUserAvatarUrl: string) => void) => {
    useEffect(() => {
        if (!userId) return;
        const realToken = localStorage.getItem("token") || "";
        const url = `${baseUrl}?access_token=${realToken}`;
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(url, {
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        const startConnection = async () => {
            try {
                await connection.start();
                connection.on("friendRequestReceived", (payload: { fromUserId: string, fromUserName: string, fromUserDisplayName: string, fromUserAvatarUrl: string }) => {
                    onFriendRequestReceived(payload.fromUserId, payload.fromUserName, payload.fromUserDisplayName, payload.fromUserAvatarUrl);
                });
            } catch (err) {
                console.error("❌ SignalR connection failed:", err);
            }
        };

        startConnection();

        return () => {
            connection.stop();
        };
    }, [userId, onFriendRequestReceived]);
};
