import { ApiResponse } from 'types/apiResponse'
import { channelApi } from 'Connections/Api/useAPIClient'
import { CreateServer, JoinServerByInviteLinkRequest, Server, ServerDetail } from 'types';
import { spreadSearchQuery } from 'utilities';

const baseUrl = '/server';

const fetchServers = async (params: any): Promise<ApiResponse<Server[]>> => {
    let q = spreadSearchQuery(params);
    const response = await channelApi.get(`${baseUrl}${q}`);
    return response.data;
};

const fetchServer = async (serverId: string): Promise<ApiResponse<Server>> => {
    const response = await channelApi.get(`${baseUrl}/${serverId}`)
    return response.data
}

const fetchServerDetail = async (serverId: string): Promise<ApiResponse<ServerDetail>> => {
    const response = await channelApi.get(`${baseUrl}/Detail/${serverId}`);
    return response.data;
};

const createServer = async (payload: CreateServer): Promise<ApiResponse<string>> => {
    const response = await channelApi.post<ApiResponse<string>>(`${baseUrl}`, payload);
    return response.data;
};

const deleteServer = async (serverId: string): Promise<ApiResponse<boolean>> => {
    const response = await channelApi.delete<ApiResponse<boolean>>(`${baseUrl}/${serverId}`);
    return response.data;
}

const joinServerByInviteLink = async (data: JoinServerByInviteLinkRequest): Promise<ApiResponse<string>> => {
    const response = await channelApi.post<ApiResponse<string>>(`${baseUrl}/JoinServerByInviteLink`, data)
    return response.data;
}

export {
    fetchServers,
    fetchServer,
    fetchServerDetail,
    createServer,
    deleteServer,
    joinServerByInviteLink
}