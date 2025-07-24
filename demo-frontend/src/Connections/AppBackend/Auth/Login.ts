import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { useDispatch } from 'react-redux';
import { loginSuccess } from 'features/auth/authSlice';
import { createPresenceConnection } from 'signalr/presenceConnection';
import { LoginRequest, TokenResponse } from 'types';
import { login } from 'features/auth/authAPI';

const saveAuthDataToLocalStorage = (data: TokenResponse) => {
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('user', JSON.stringify(data.user));
};

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
