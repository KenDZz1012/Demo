import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { useDispatch } from 'react-redux';
import { loginSuccess } from 'features/auth/authSlice';
import { createPresenceConnection } from 'signalr/presenceConnection';
import { LoginRequest, LogoutRequest, TokenResponse } from 'types';
import { login, logout } from 'features/auth/authAPI';

const saveAuthDataToLocalStorage = (data: TokenResponse) => {
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('user', JSON.stringify(data.user));
};

const removeAuthDataFromLocalStorage = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
}


export const useLogin = (): UseMutationResult<TokenResponse, Error, LoginRequest> => {
    const queryClient = useQueryClient();
    const dispatch = useDispatch();

    return useMutation<TokenResponse, Error, LoginRequest>({
        mutationFn: async (loginData: LoginRequest): Promise<TokenResponse> => {
            const response = await login(loginData);
            if (!response.isSuccess) {
                throw new Error(response.message || 'Login failed');
            }
            saveAuthDataToLocalStorage(response.data);
            return response.data;
        },
        onSuccess: async (data) => {
            dispatch(loginSuccess(data.user));
            queryClient.invalidateQueries({ queryKey: ['users'] });
            try {
                await createPresenceConnection(data.accessToken);
            } catch (error) {
                console.error('Failed to connect to SignalR PresenceHub:', error);
            }
        },
        onError: (error: Error) => {
            console.error('Login failed:', error);
        }
    });
};

export const useLogout = (): UseMutationResult<boolean, Error, LogoutRequest> => {
    return useMutation<boolean, Error, LogoutRequest>({
        mutationFn: async (loginData: LogoutRequest): Promise<boolean> => {
            const response = await logout(loginData);
            if (!response.isSuccess) {
                throw new Error(response.message || 'Login failed');
            }
            return response.data;
        },
        onSuccess: async (data) => {
            removeAuthDataFromLocalStorage();
        },
        onError: (error: Error) => {
        }
    });
}
