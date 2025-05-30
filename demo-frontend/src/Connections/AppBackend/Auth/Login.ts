import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { authApi, userApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
export interface Login {
    userName: string;
    password: string;
}



export const useLogin = (): UseMutationResult<boolean, Error, Login> => {
    const queryClient = useQueryClient();

    return useMutation<boolean, Error, Login>({
        mutationFn: async (newUser: Login): Promise<boolean> => {
            const response = await authApi.post<ApiResponse<boolean>>('/login', newUser);
            if (!response.data.isSuccess) {
                throw new Error(response.data.message || 'Login failed');
            }
            return response.data.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};