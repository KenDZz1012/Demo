import { addFriendPending } from 'features/user-relationship/userRelationshipSlice';
import { useSignalREvent } from 'hooks/useSignalREvent';
import { useDispatch } from 'react-redux';

const FriendRequestListener = () => {
    const dispatch = useDispatch();

    useSignalREvent('friendRequestReceived', (payload: any) => {
        dispatch(addFriendPending({
            id: payload.fromUserId,
            userName: payload.fromUserName,
            displayName: payload.fromUserDisplayName,
            isSender: true,
            avatarUrl: payload.fromUserAvatarUrl || '',
        }));
    });

    useSignalREvent('friendRequestAccepted', (payload: any) => {
        console.log('Friend Request Accepted:', payload);
    });

    useSignalREvent('friendRequestCancelled', (payload: any) => { })

    return null;
};

export default FriendRequestListener;
