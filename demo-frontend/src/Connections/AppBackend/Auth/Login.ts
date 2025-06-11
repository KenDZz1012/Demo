import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { authApi, userApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
import { useDispatch } from 'react-redux';
import { loginSuccess } from '../../../features/auth/authSlice';
import { createPresenceConnection } from '../../../signalr/presenceConnection';

export interface Login {
    userName: string;
    password: string;
}

export interface TokenResponse {
    accessToken: string;
    refreshToken: string;
}


export const useLogin = (): UseMutationResult<TokenResponse, Error, Login> => {
    const queryClient = useQueryClient();
    const dispatch = useDispatch();

    return useMutation<TokenResponse, Error, Login>({
        mutationFn: async (newUser: Login): Promise<TokenResponse> => {
            const response = await authApi.post<ApiResponse<TokenResponse>>('/login', newUser);
            if (!response.data.isSuccess) {
                throw new Error(response.data.message || 'Login failed');
            }
            // Lưu token vào localStorage nếu có
            if (response.data.data.accessToken) {
                localStorage.setItem('token', response.data.data.accessToken);
            }
            return response.data.data;
        },
        onSuccess: async (_data, variables) => {
            dispatch(loginSuccess(variables.userName));
            queryClient.invalidateQueries({ queryKey: ['users'] });
            try {
                await createPresenceConnection();
            } catch (error) {
                console.error('Failed to connect to SignalR PresenceHub', error);
            }
        },
    });
};