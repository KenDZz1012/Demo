import { ApiResponse } from 'types/apiResponse'
import { userApi } from 'Connections/Api/useAPIClient'
import { AddFriendRequest, CancelFriendRequest, Friend, FriendPending, UpdateUserRelationship } from 'types/user';
import { spreadSearchQuery } from 'utilities';

const baseUrl = '/UserRelationship';

const fetchFriends = async (params: any): Promise<ApiResponse<Friend[]>> => {
    let q = spreadSearchQuery(params);
    const response = await userApi.get(`${baseUrl}/Friends${q}`);
    return response.data;
};

const addFriend = async (data: AddFriendRequest): Promise<ApiResponse<string>> => {
    const response = await userApi.post(`${baseUrl}`, data);
    return response.data;
}

const fetchFriendsPending = async (params: any): Promise<ApiResponse<FriendPending[]>> => {
    let q = spreadSearchQuery(params);
    const response = await userApi.get(`${baseUrl}/FriendsPending${q}`);
    return response.data;
}

const cancelFriendRequest = async (data: CancelFriendRequest): Promise<ApiResponse<string>> => {
    const response = await userApi.put(`${baseUrl}`, data);
    return response.data;
}

const updateUserRelationship = async (data: UpdateUserRelationship): Promise<ApiResponse<string>> => {
    const response = await userApi.put(`${baseUrl}`, data);
    return response.data;
}

export {
    fetchFriends,
    addFriend,
    fetchFriendsPending,
    cancelFriendRequest,
    updateUserRelationship
}