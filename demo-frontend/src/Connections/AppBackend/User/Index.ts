import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { userApi } from '../../Api/client';

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
    password: string;
    displayName?: string;
    dateOfBirth?: Date;
}


export const useCreateUser = (): UseMutationResult<User, Error, CreateUserInput> => {
    const queryClient = useQueryClient();

    return useMutation<User, Error, CreateUserInput>({
        mutationFn: async (newUser: CreateUserInput): Promise<User> => {
            const response = await userApi.post<User>('/user', newUser);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};