import { ApiResponse } from 'types/apiResponse'
import { channelApi } from 'Connections/Api/client'
import { CreateServer, Server, ServerDetail } from 'types';
import { spreadSearchQuery } from 'utilities';

const fetchServers = async (params: any): Promise<ApiResponse<Server[]>> => {
    let q = spreadSearchQuery(params);
    const response = await channelApi.get(`/server${q}`);
    return response.data;
};

const fetchServerDetail = async (serverId: string): Promise<ApiResponse<ServerDetail>> => {
    const response = await channelApi.get(`/server/${serverId}`);
    return response.data;
};

const createServer = async (payload: CreateServer): Promise<ApiResponse<string>> => {
    const response = await channelApi.post<ApiResponse<string>>('/server', payload);
    return response.data;
};

export {
    fetchServers,
    fetchServerDetail,
    createServer
}