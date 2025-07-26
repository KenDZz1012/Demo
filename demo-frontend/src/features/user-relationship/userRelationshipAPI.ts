import { LoginRequest, TokenResponse } from 'types'
import { ApiResponse } from 'types/apiResponse'
import { userApi } from 'Connections/Api/client'
import { AddFriendRequest, Friend, FriendPending } from 'types/user';
import { spreadSearchQuery } from 'utilities';


const fetchFriends = async (params: any): Promise<ApiResponse<Friend[]>> => {
    let q = spreadSearchQuery(params);
    const response = await userApi.get(`/UserRelationship/Friends${q}`);
    return response.data;
};

const addFriend = async (data: AddFriendRequest): Promise<ApiResponse<string>> => {
    const response = await userApi.post('/UserRelationship', data);
    return response.data;
}

const fetchFriendsPending = async (params: any): Promise<ApiResponse<FriendPending[]>> => {
    let q = spreadSearchQuery(params);
    const response = await userApi.get(`/UserRelationship/FriendsPending${q}`);
    return response.data;
}

export {
    fetchFriends,
    addFriend,
    fetchFriendsPending
}