import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { useDispatch } from 'react-redux';
import { loginSuccess } from 'features/auth/authSlice';
import { createPresenceConnection } from 'signalr/presenceConnection';
import { LoginRequest, TokenResponse } from 'types';
import { login } from 'features/auth/authAPI';


export const useLogin = (): UseMutationResult<TokenResponse, Error, LoginRequest> => {
    const queryClient = useQueryClient();
    const dispatch = useDispatch();

    return useMutation<TokenResponse, Error, LoginRequest>({
        mutationFn: async (newUser: LoginRequest): Promise<TokenResponse> => {
            const response = await login(newUser);
            if (!response.isSuccess) {
                throw new Error(response.message || 'Login failed');
            }
            if (response.data.accessToken) {
                localStorage.setItem('token', response.data.accessToken);
                localStorage.setItem('refreshToken', response.data.refreshToken);
                localStorage.setItem("userID", response.data.userID);
            }
            return response.data;
        },
        onSuccess: async (_data, variables) => {
            dispatch(loginSuccess(variables.userName));
            queryClient.invalidateQueries({ queryKey: ['users'] });
            try {
                await createPresenceConnection(_data.accessToken);
            } catch (error) {
                console.error('Failed to connect to SignalR PresenceHub', error);
            }
        },
        onError: async (error: Error) => {
            return error;
        }
    });
};