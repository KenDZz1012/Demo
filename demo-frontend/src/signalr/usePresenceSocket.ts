import { useEffect } from 'react';
import { createPresenceConnection, onEvent, offEvent, stopPresenceConnection } from './signalrService';

export const usePresenceSocket = (
    userId: string,
    onFriendRequestReceived: (fromUserId: string, fromUserName: string, fromUserDisplayName: string, fromUserAvatarUrl: string) => void
) => {
    useEffect(() => {
        if (!userId) return;

        const setupConnection = async () => {
            await createPresenceConnection();
            onEvent("friendRequestReceived", (payload: {
                fromUserId: string;
                fromUserName: string;
                fromUserDisplayName: string;
                fromUserAvatarUrl: string;
            }) => {
                onFriendRequestReceived(payload.fromUserId, payload.fromUserName, payload.fromUserDisplayName, payload.fromUserAvatarUrl);
            });
        };

        setupConnection();

        return () => {
            offEvent("friendRequestReceived");
            stopPresenceConnection();
        };
    }, [userId, onFriendRequestReceived]);
};
