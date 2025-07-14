import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from '@tanstack/react-query';
import { channelApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
import { CreateServer, Server } from '../../Types/Channel';
import { AxiosError } from 'axios';

export const useServers = (): UseQueryResult<ApiResponse<Server[]>, Error> =>
    useQuery({
        queryKey: ['servers'],
        queryFn: () => channelApi.get("/server").then(r => r.data),
    });

export const useCreateServer = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, CreateServer> => {
    const queryClient = useQueryClient();

    return useMutation<string, AxiosError<ApiResponse<string>>, CreateServer>({
        mutationFn: async (newUser: CreateServer): Promise<string> => {
            const response = await channelApi.post<ApiResponse<string>>('/server', newUser);
            if (!response.data.isSuccess) {
                throw new AxiosError(response.data.message || 'Create server failed');
            }
            return response.data.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['users'] });
        },
    });
};