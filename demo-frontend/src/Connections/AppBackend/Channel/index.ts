import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from '@tanstack/react-query';
import { CreateChannel, CreateServer, JoinServerByInviteLinkRequest, Server, ServerDetail } from 'types';
import { AxiosError } from 'axios';
import { useNavigate } from 'react-router-dom';
import { ApiResponse } from 'types/apiResponse';
import { fetchServers, fetchServerDetail, createServer, deleteServer, joinServerByInviteLink, fetchServer } from 'features/server/serverAPI'
import { useDispatch, useSelector } from 'react-redux';
import { selectAuthUser } from 'store/selectors/authSelectors';
import { addChannel, addServer, removeServer, setSelectedServer, setSelectedServerId } from 'features/server/serverSlice';
import { fetchChannel } from 'features/channel/channelAPI';


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
    const dispatch = useDispatch();
    const navigate = useNavigate();
    return useMutation<ApiResponse<string>, AxiosError<ApiResponse<string>>, CreateServer>({
        mutationFn: createServer,
        onSuccess: async (data) => {
            const detailRes = await fetchServerDetail(data.data);
            if (!detailRes.isSuccess) return;
            const newServer = detailRes.data;
            dispatch(addServer(newServer));
            dispatch(setSelectedServer(newServer))
            dispatch(setSelectedServerId(newServer.id))
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
    const dispatch = useDispatch();

    return useMutation<ApiResponse<boolean>, AxiosError<ApiResponse<boolean>>, string>({
        mutationFn: deleteServer,
        onSuccess: (data, deletedServerId) => {
            dispatch(removeServer(deletedServerId))
        }
    });
};

export const useJoinServerByInviteLink = (onSuccessCallback?: () => void): UseMutationResult<ApiResponse<string>, AxiosError<ApiResponse<string>>, JoinServerByInviteLinkRequest> => {
    const navigate = useNavigate();
    const dispatch = useDispatch();
    return useMutation<ApiResponse<string>, AxiosError<ApiResponse<string>>, JoinServerByInviteLinkRequest>({
        mutationFn: joinServerByInviteLink,
        onSuccess: async (data) => {
            const detailRes = await fetchServer(data.data);
            if (!detailRes.isSuccess) return;
            const newServer = detailRes.data;
            dispatch(addServer(newServer));
            navigate(`/server/${newServer.id}`);
            onSuccessCallback?.();
        },
    });
}


export const useCreateChannel = (onSuccessCallback?: () => void): UseMutationResult<ApiResponse<string>, AxiosError<ApiResponse<string>>, CreateChannel> => {
    const dispatch = useDispatch();
    return useMutation<ApiResponse<string>, AxiosError<ApiResponse<string>>, CreateChannel>({
        mutationFn: createServer,
        onSuccess: async (data) => {
            const detailRes = await fetchChannel(data.data);
            if (!detailRes.isSuccess) return;
            const newChannel = detailRes.data;
            dispatch(addChannel(newChannel));
            onSuccessCallback?.();
        },
    });
}
