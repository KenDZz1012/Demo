import { LoginRequest, TokenResponse } from 'types'
import { ApiResponse } from 'types/apiResponse'
import { userApi } from 'Connections/Api/client'
import { Friend } from 'types/user';
import { spreadSearchQuery } from 'utilities';


const fetchFriends = async (params: any): Promise<ApiResponse<Friend[]>> => {
    let q = spreadSearchQuery(params);
    const response = await userApi.get(`/UserRelationship/Friends${q}`);
    return response.data;
};

export {
    fetchFriends
}