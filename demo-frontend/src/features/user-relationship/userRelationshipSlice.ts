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
        setFriendsPending: (state, action: PayloadAction<FriendPending[]>) => {
            state.friensPending = action.payload;
        },
    },
});

export const { setFriends, setFriendsPending } = userRelationshipSlice.actions;
export default userRelationshipSlice.reducer;
