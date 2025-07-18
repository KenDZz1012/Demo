import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { userApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
import axios, { AxiosError } from 'axios';

export interface User {
    id: string;
    userName: string;
    displayName: string;
    email: string;
    status: string;
    avatarUrl: string;
    dateOfBirth: Date;
}

export interface CreateUserInput {
    userName: string;
    email: string;
    passwordHash: string;
    displayName?: string;
    dateOfBirth?: string;
    isAdmin?: boolean;
}


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

