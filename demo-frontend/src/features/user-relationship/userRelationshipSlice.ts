import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { stat } from 'fs';
import { Friend, FriendPending } from 'types/user';

interface FriendState {
    friends: Friend[];
    friensPending: FriendPending[];
    selectedFriendId?: string | null;
    selectedFriend?: Friend | null;
}

const initialState: FriendState = {
    friends: [],
    friensPending: [],
    selectedFriendId: null,
    selectedFriend: null,
};


const userRelationshipSlice = createSlice({
    name: 'userRelationship',
    initialState,
    reducers: {
        setFriends: (state, action: PayloadAction<Friend[]>) => {
            state.friends = action.payload;
        },
        addFriend: (state, action: PayloadAction<Friend>) => {
            const exists = state.friends.some(friend => friend.id === action.payload.id);
            if (!exists) {
                state.friends.push(action.payload);
            }
        },
        setFriendsPending: (state, action: PayloadAction<FriendPending[]>) => {
            state.friensPending = action.payload;
        },
        addFriendPending: (state, action: PayloadAction<FriendPending>) => {
            const exists = state.friensPending.some(friend => friend.id === action.payload.id);
            if (!exists) {
                state.friensPending.push(action.payload);
            }
        },
        removeFriendPending: (state, action: PayloadAction<string>) => {
            state.friensPending = state.friensPending.filter(friend => friend.id !== action.payload);
        },
        setStatusFriend: (state, action: PayloadAction<{ userName: string; isOnline: boolean }>) => {
            let friend = state.friends.find(friend => friend.userName === action.payload.userName);
            if (friend) {
                friend.isOnline = action.payload.isOnline;
            }
        },
        setSelectedFriend: (state, action: PayloadAction<Friend | null>) => {
            state.selectedFriendId = action.payload?.id;
            state.selectedFriend = action.payload || null;
        }
    },
});

export const { setFriends, setFriendsPending, addFriendPending, removeFriendPending, addFriend, setStatusFriend, setSelectedFriend } = userRelationshipSlice.actions;
export default userRelationshipSlice.reducer;
