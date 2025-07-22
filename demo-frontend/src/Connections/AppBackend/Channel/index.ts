import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from '@tanstack/react-query';
import { CreateServer, Server, ServerDetail } from 'types';
import { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { ApiResponse } from 'types/apiResponse';
import { channelApi } from 'Connections/Api/client';

// Query lấy danh sách server
const fetchServers = async (): Promise<ApiResponse<Server[]>> => {
    const ownerId = localStorage.getItem("userID")?.toString();
    const response = await channelApi.get(`/server?OwnerId=${ownerId}`);
    return response.data;
};

export const useServers = (): UseQueryResult<ApiResponse<Server[]>, Error> =>
    useQuery({
        queryKey: ['servers'],
        queryFn: fetchServers,
    });

// Query lấy chi tiết server
const fetchServerDetail = async (serverId: string): Promise<ApiResponse<ServerDetail>> => {
    const response = await channelApi.get(`/server/${serverId}`);
    return response.data;
};

export const useServer = (serverId: string): UseQueryResult<ApiResponse<ServerDetail>, Error> =>
    useQuery({
        queryKey: ['server', serverId],
        queryFn: () => fetchServerDetail(serverId),
        enabled: !!serverId,
    });

// Mutation tạo server mới
const createServer = async (payload: CreateServer): Promise<string> => {
    const response = await channelApi.post<ApiResponse<string>>('/server', payload);
    if (!response.data.isSuccess) {
        throw new AxiosError(response.data.message || 'Create server failed');
    }
    return response.data.data;
};

export const useCreateServer = (
    onSuccessCallback?: () => void
): UseMutationResult<string, AxiosError<ApiResponse<string>>, CreateServer> => {
    const queryClient = useQueryClient();
    const navigate = useNavigate();

    return useMutation<string, AxiosError<ApiResponse<string>>, CreateServer>({
        mutationFn: createServer,
        onSuccess: async (serverId) => {
            const detailRes = await channelApi.get<ApiResponse<Server>>(`/server/${serverId}`);
            if (!detailRes.data.isSuccess) return;

            const newServer = detailRes.data.data;

            queryClient.setQueryData<ApiResponse<Server[]>>(['servers'], (old) => {
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