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

export const useServer = (serverId: string): UseQueryResult<ApiResponse<Server>, Error> =>
    useQuery({
        queryKey: ['server', serverId],
        queryFn: () => channelApi.get(`/server/${serverId}`).then(r => r.data),
        enabled: !!serverId,
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
        onSuccess: async (serverId) => {

            // Gọi lại API để lấy chi tiết server theo id
            const serverDetailRes = await channelApi.get<ApiResponse<Server>>(`/server/${serverId}`);
            if (!serverDetailRes.data.isSuccess) return;

            // Lấy danh sách hiện tại từ cache
            const prev = queryClient.getQueryData<ApiResponse<Server[]>>(['server']);
            const newServer = serverDetailRes.data.data;

            // Cập nhật cache danh sách server
            queryClient.setQueryData<ApiResponse<Server[]>>(['server'], (old) => {
                if (!old || !old.data) {
                    return {
                        isSuccess: true,
                        data: [newServer],
                        message: 'Created new server',
                    };
                }

                return {
                    ...old,
                    data: [...old.data, newServer],
                    message: old.message || 'Updated server list',
                };
            });
        },
    });
};