import { useMutation, useQueryClient, UseMutationResult } from '@tanstack/react-query';
import { ApiResponse } from '../../../types/apiResponse';
import { AxiosError } from 'axios';
import { CreateUserInput } from 'types/user';
import { createUser } from 'features/user/userAPI';


export const useCreateUser = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, CreateUserInput> => {
    const queryClient = useQueryClient();

    return useMutation<string, AxiosError<ApiResponse<string>>, CreateUserInput>({
        mutationFn: async (newUser: CreateUserInput): Promise<string> => {
            const response = await createUser(newUser);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Create user failed');
            }
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};

