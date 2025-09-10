import { ApiResponse } from 'types/apiResponse'
import { directMessageApi } from 'Connections/Api/useAPIClient'
import { spreadSearchQuery } from 'utilities';
import { SendMessageRequest } from 'types';

const baseUrl = '/DirectMessage';

const sendMessage = async (data: SendMessageRequest): Promise<ApiResponse<string>> => {
    const response = await directMessageApi.post(`${baseUrl}/SendMessage`, data);
    return response.data;
};

export {
    sendMessage
}