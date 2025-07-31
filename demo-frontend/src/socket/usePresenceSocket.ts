import { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

let connection: signalR.HubConnection;

export const usePresenceSocket = (userId: string, onFriendRequestReceived: (fromUserId: string) => void) => {
    useEffect(() => {
        if (!userId) return;

        connection = new signalR.HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_URL_PRESENCE}/presenceHub`, {
                accessTokenFactory: () => localStorage.getItem('token') || ''
            })
            .withAutomaticReconnect()
            .build();

        const startConnection = async () => {
            try {
                await connection.start();
                console.log("✅ SignalR connected to PresenceHub");

                connection.on("friendRequestReceived", (payload: { fromUserId: string }) => {
                    console.log("📥 Friend request from:", payload.fromUserId);
                    onFriendRequestReceived(payload.fromUserId);
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
