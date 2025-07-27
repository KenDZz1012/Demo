import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from "@tanstack/react-query";
import { AxiosError } from "axios";
import { userRelationshipStatus } from "shared";
import { addFriend, cancelFriendRequest, fetchFriends, fetchFriendsPending, updateUserRelationship } from "features/user-relationship/userRelationshipAPI";
import { ApiResponse } from "types/apiResponse";
import { AddFriendRequest, CancelFriendRequest, Friend, FriendPending, UpdateUserRelationship } from "types/user";

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

export const useCancelFriendRequest = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, CancelFriendRequest> => {
    const queryClient = useQueryClient();
    return useMutation<string, AxiosError<ApiResponse<string>>, CancelFriendRequest>({
        mutationFn: async (data: CancelFriendRequest): Promise<string> => {
            const response = await cancelFriendRequest(data);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Cancel friend request failed');
            }
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['friendsPending'] });
        },
    });
}


export const useUpdateUserRelationship = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, UpdateUserRelationship> => {
    const queryClient = useQueryClient();
    return useMutation<string, AxiosError<ApiResponse<string>>, UpdateUserRelationship>({
        mutationFn: async (data: UpdateUserRelationship): Promise<string> => {
            const response = await updateUserRelationship(data);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Update user relationship failed');
            }
            return response.data;
        },
        onSuccess: (_data, variables) => {
            queryClient.invalidateQueries({ queryKey: ['friendsPending'] });

            if (variables.status === userRelationshipStatus.Accepted) {
                queryClient.invalidateQueries({ queryKey: ['friends'] });
            }
        },
    });
}