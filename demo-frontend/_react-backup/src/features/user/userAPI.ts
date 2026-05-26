import { ApiResponse } from 'types/apiResponse'
import { userApi } from 'Connections/Api/useAPIClient'
import { CreateUserInput } from 'types/user';

const baseUrl = '/user';

export const createUser = async (data: CreateUserInput): Promise<ApiResponse<string>> => {
    const response = await userApi.post<ApiResponse<string>>(`${baseUrl}`, data);
    return response.data;
}