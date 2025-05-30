import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { userApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
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
    dateOfBirth?: Date;
    isAdmin?: boolean;
}


export const useCreateUser = (): UseMutationResult<string, Error, CreateUserInput> => {
    const queryClient = useQueryClient();

    return useMutation<string, Error, CreateUserInput>({
        mutationFn: async (newUser: CreateUserInput): Promise<string> => {
            const response = await userApi.post<ApiResponse<string>>('/user', newUser);
            if (!response.data.isSuccess) {
                throw new Error(response.data.message || 'Create user failed');
            }
            return response.data.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};