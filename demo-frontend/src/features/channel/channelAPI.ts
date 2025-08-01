import { ApiResponse } from 'types/apiResponse'
import { channelApi } from 'Connections/Api/useAPIClient'
import { CreateChannel } from 'types';

const baseUrl = '/channel';

const createChannel = async (payload: CreateChannel): Promise<ApiResponse<string>> => {
    const response = await channelApi.post<ApiResponse<string>>(`${baseUrl}`, payload);
    return response.data;
};

export {
    createChannel
}