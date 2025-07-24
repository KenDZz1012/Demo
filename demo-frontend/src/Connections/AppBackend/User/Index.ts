import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { userApi } from '../../Api/client';
import { ApiResponse } from '../../../types/apiResponse';
import axios, { AxiosError } from 'axios';
import { CreateUserInput } from 'types/user';


export const useCreateUser = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, CreateUserInput> => {
    const queryClient = useQueryClient();

    return useMutation<string, AxiosError<ApiResponse<string>>, CreateUserInput>({
        mutationFn: async (newUser: CreateUserInput): Promise<string> => {
            const response = await userApi.post<ApiResponse<string>>('/user', newUser);
            if (!response.data.isSuccess) {
                throw new AxiosError(response.data.message || 'Create user failed');
            }
            return response.data.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};

