import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { authApi, userApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
import { useDispatch } from 'react-redux';
import { loginSuccess } from '../../../features/auth/authSlice';

export interface Login {
    userName: string;
    password: string;
}

export const useLogin = (): UseMutationResult<boolean, Error, Login> => {
    const queryClient = useQueryClient();
    const dispatch = useDispatch();

    return useMutation<boolean, Error, Login>({
        mutationFn: async (newUser: Login): Promise<boolean> => {
            const response = await authApi.post<ApiResponse<boolean>>('/login', newUser);
            if (!response.data.isSuccess) {
                throw new Error(response.data.message || 'Login failed');
            }
            return response.data.data;
        },
        onSuccess: (_data, variables) => {
            dispatch(loginSuccess(variables.userName));
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};