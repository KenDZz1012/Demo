import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Friend } from 'types/user';

interface FriendState {
    friends: Friend[];
}

const initialState: FriendState = {
    friends: [],
};


const userRelationshipSlice = createSlice({
    name: 'userRelationship',
    initialState,
    reducers: {
        setFriends: (state, action: PayloadAction<Friend[]>) => {
            state.friends = action.payload;
        },
    },
});

export const { setFriends } = userRelationshipSlice.actions;
export default userRelationshipSlice.reducer;
