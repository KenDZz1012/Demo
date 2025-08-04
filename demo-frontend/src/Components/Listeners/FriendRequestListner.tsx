import { addFriend, addFriendPending, removeFriendPending } from 'features/user-relationship/userRelationshipSlice';
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
        dispatch(addFriend({
            id: payload.fromUserId,
            userName: payload.fromUserName,
            displayName: payload.fromUserDisplayName,
            avatarUrl: payload.fromUserAvatarUrl || '',
            isOnline: payload.isOnline,
        }));

        dispatch(removeFriendPending(payload.fromUserId));
    });

    useSignalREvent('friendRequestRejected', (payload: any) => {
        dispatch(removeFriendPending(payload.fromUserId));
    })

    return null;
};

export default FriendRequestListener;
