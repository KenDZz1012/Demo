import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AuthState } from 'types';
import { AuthUser } from 'types/user';

const initialState: AuthState = {
    user: null,
    isAuthenticated: false,
    accessToken: null,
    refreshToken: null,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        loginSuccess(state, action: PayloadAction<AuthUser>) {
            state.user = action.payload;
            state.isAuthenticated = true;
        },
        logoutSuccess(state) {
            state.user = null;
            state.isAuthenticated = false;
            state.accessToken = null;
            state.refreshToken = null;
        },
    },
});

export const { loginSuccess, logoutSuccess } = authSlice.actions;
export default authSlice.reducer;
