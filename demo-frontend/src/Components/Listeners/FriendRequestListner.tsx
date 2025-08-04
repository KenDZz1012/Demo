import { useSignalREvent } from 'hooks/useSignalREvent';

const FriendRequestListener = () => {
    useSignalREvent('friendRequestReceived', (payload: any) => {
        console.log('Friend Request Received:', payload);
    });

    useSignalREvent('friendRequestAccepted', (payload: any) => {
        console.log('Friend Request Accepted:', payload);
    });

    return null;
};

export default FriendRequestListener;
