import { addFriend, addFriendPending, removeFriendPending, setStatusFriend } from 'features/user-relationship/userRelationshipSlice';
import { useSignalREvent } from 'hooks/useSignalREvent';
import { useDispatch } from 'react-redux';

const FriendStatusListener = () => {
    const dispatch = useDispatch();
    useSignalREvent('friendStatusChanged', (payload: any) => {
        dispatch(setStatusFriend({
            userName: payload.userName,
            isOnline: payload.isOnline,
        }));
    })
    return null;
}

export default FriendStatusListener;
