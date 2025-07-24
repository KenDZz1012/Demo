import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from '@tanstack/react-query';
import { CreateServer, Server, ServerDetail } from 'types';
import { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { ApiResponse } from 'types/apiResponse';
import { channelApi } from 'Connections/Api/client';
import { fetchServers, fetchServerDetail, createServer } from 'features/server/serverAPI'


export const useServers = (params: any): UseQueryResult<ApiResponse<Server[]>, Error> =>
    useQuery({
        queryKey: ['servers', params],
        queryFn: () => fetchServers(params),
        staleTime: 1000 * 60 * 5,
    });


export const useServer = (serverId: string): UseQueryResult<ApiResponse<ServerDetail>, Error> =>
    useQuery({
        queryKey: ['server', serverId],
        queryFn: () => fetchServerDetail(serverId),
        enabled: !!serverId,
    });


export const useCreateServer = (
    onSuccessCallback?: () => void
): UseMutationResult<ApiResponse<string>, AxiosError<ApiResponse<string>>, CreateServer> => {
    const queryClient = useQueryClient();
    const navigate = useNavigate();

    return useMutation<ApiResponse<string>, AxiosError<ApiResponse<string>>, CreateServer>({
        mutationFn: createServer,
        onSuccess: async (data) => {
            const detailRes = await channelApi.get<ApiResponse<Server>>(`/server/${data.data}`);
            if (!detailRes.data.isSuccess) return;
            const newServer = detailRes.data.data;
            queryClient.setQueryData<ApiResponse<Server[]>>(['servers', { ownerId: newServer.ownerId }], (old) => {
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
                };
            });
            navigate(`/server/${newServer.id}`);
            onSuccessCallback?.();
        },
    });
};