import { createUserApi, createAuthApi, createChannelApi } from './client';
import { store } from 'app/store';
import { removeAuthDataFromLocalStorage } from 'Connections/AppBackend/Auth';
import { logoutSuccess } from 'features/auth/authSlice';

const onLogout = () => {
    removeAuthDataFromLocalStorage();
    store.dispatch(logoutSuccess());
    window.location.href = '/login';
};

export const userApi = createUserApi(onLogout);
export const authApi = createAuthApi(onLogout);
export const channelApi = createChannelApi(onLogout);
