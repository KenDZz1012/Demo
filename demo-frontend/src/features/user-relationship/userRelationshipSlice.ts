import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Friend, FriendPending } from 'types/user';

interface FriendState {
    friends: Friend[];
    friensPending: FriendPending[];
}

const initialState: FriendState = {
    friends: [],
    friensPending: [],
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

    },
});

export const { setFriends, setFriendsPending, addFriendPending, removeFriendPending, addFriend } = userRelationshipSlice.actions;
export default userRelationshipSlice.reducer;
