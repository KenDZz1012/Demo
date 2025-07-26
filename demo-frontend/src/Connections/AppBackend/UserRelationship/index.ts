import { useMutation, UseMutationResult, useQuery, UseQueryResult } from "@tanstack/react-query";
import { AxiosError } from "axios";
import { addFriend, fetchFriends, fetchFriendsPending } from "features/user-relationship/userRelationshipAPI";
import { ApiResponse } from "types/apiResponse";
import { AddFriendRequest, Friend, FriendPending } from "types/user";

export const useFriends = (params: any): UseQueryResult<ApiResponse<Friend[]>, Error> =>
    useQuery({
        queryKey: ['friends', params],
        queryFn: () => fetchFriends(params),
    });

export const useFriendsPending = (params: any): UseQueryResult<ApiResponse<FriendPending[]>, Error> =>
    useQuery({
        queryKey: ['friendsPending', params],
        queryFn: () => fetchFriendsPending(params),
    });

export const useAddFriend = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, AddFriendRequest> => {
    return useMutation<string, AxiosError<ApiResponse<string>>, AddFriendRequest>({
        mutationFn: async (newUser: AddFriendRequest): Promise<string> => {
            const response = await addFriend(newUser);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Create user failed');
            }
            return response.data;
        },
    });
};