import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface AuthState {
    userName: string | null;
    isLoggedIn: boolean;
}

const initialState: AuthState = {
    userName: null,
    isLoggedIn: false,
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        loginSuccess(state, action: PayloadAction<string>) {
            state.userName = action.payload;
            state.isLoggedIn = true;
        },
        logout(state) {
            state.userName = null;
            state.isLoggedIn = false;
        },
    },
});

export const { loginSuccess, logout } = authSlice.actions;
export default authSlice.reducer;
