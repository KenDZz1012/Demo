import { ApiResponse } from 'types/apiResponse'
import { userApi } from 'Connections/Api/client'
import { CreateUserInput } from 'types/user';

export const createUser = async (data: CreateUserInput): Promise<ApiResponse<string>> => {
    const response = await userApi.post<ApiResponse<string>>('/user', data);
    return response.data;
}