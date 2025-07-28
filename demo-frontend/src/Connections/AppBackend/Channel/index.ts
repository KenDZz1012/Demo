import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from '@tanstack/react-query';
import { CreateServer, JoinServerByInviteLinkRequest, Server, ServerDetail } from 'types';
import { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { ApiResponse } from 'types/apiResponse';
import { fetchServers, fetchServerDetail, createServer, deleteServer, joinServerByInviteLink } from 'features/server/serverAPI'
import { useSelector } from 'react-redux';
import { selectAuthUser } from 'store/selectors/authSelectors';


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
            const detailRes = await fetchServerDetail(data.data);
            if (!detailRes.isSuccess) return;
            const newServer = detailRes.data;
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

export const useDeleteServer = (): UseMutationResult<
    ApiResponse<boolean>,
    AxiosError<ApiResponse<boolean>>,
    string
> => {
    const queryClient = useQueryClient();

    return useMutation<ApiResponse<boolean>, AxiosError<ApiResponse<boolean>>, string>({
        mutationFn: deleteServer,
        onSuccess: (data, deletedServerId) => {
            queryClient.setQueriesData<ApiResponse<Server[]>>(
                { queryKey: ['servers'] },
                (old) => {
                    if (!old || !old.data) return old;
                    return {
                        ...old,
                        data: old.data.filter((s) => s.id !== deletedServerId),
                    };
                }
            );
        }
    });
};

export const useJoinServerByInviteLink = (onSuccessCallback?: () => void): UseMutationResult<ApiResponse<string>, AxiosError<ApiResponse<string>>, JoinServerByInviteLinkRequest> => {
    const queryClient = useQueryClient();
    const navigate = useNavigate();
    const currentUserId = useSelector(selectAuthUser)?.id;
    return useMutation<ApiResponse<string>, AxiosError<ApiResponse<string>>, JoinServerByInviteLinkRequest>({
        mutationFn: joinServerByInviteLink,
        onSuccess: async (data) => {
            const detailRes = await fetchServerDetail(data.data);
            if (!detailRes.isSuccess) return;
            const newServer = detailRes.data;
            queryClient.setQueryData<ApiResponse<Server[]>>(['servers', { ownerId: currentUserId }], (old) => {
                if (!old || !old.data) {
                    return {
                        isSuccess: true,
                        data: [newServer],
                        message: 'Join new server',
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
}
