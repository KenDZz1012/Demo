import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from "@tanstack/react-query";
import { useDispatch, useSelector } from "react-redux";
import { AxiosError } from "axios";
import { userRelationshipStatus } from "shared";
import { addFriend, cancelFriendRequest, fetchFriends, fetchFriendsPending, updateUserRelationship } from "features/user-relationship/userRelationshipAPI";
import { addFriend as addFriendSlice, addFriendPending, removeFriendPending, setFriends, setFriendsPending } from "features/user-relationship/userRelationshipSlice";
import { ApiResponse } from "types/apiResponse";
import { AddFriendRequest, CancelFriendRequest, CreateUserRelationshipResponse, Friend, FriendPending, UpdateUserRelationship, UpdateUserRelationshipResponse } from "types/user";
import { selectFriends, selectFriendsPending } from "store/selectors/authSelectors";

// Giữ nguyên useFriends và useFriendsPending
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

export const useAddFriend = (): UseMutationResult<CreateUserRelationshipResponse, AxiosError<ApiResponse<CreateUserRelationshipResponse>>, AddFriendRequest> => {
    const dispatch = useDispatch();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (newUser: AddFriendRequest): Promise<CreateUserRelationshipResponse> => {
            const response = await addFriend(newUser);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Create user failed');
            }
            return response.data;
        },
        onSuccess: (data, variables) => {
            const newFriend: FriendPending = {
                id: data.id,
                userName: data.userName,
                displayName: data.displayName,
                avatarUrl: data.avatarUrl,
                isSender: false,
            };
            dispatch(addFriendPending(newFriend));
        }
    });
};

export const useCancelFriendRequest = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, CancelFriendRequest> => {
    const dispatch = useDispatch();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (data: CancelFriendRequest): Promise<string> => {
            const response = await cancelFriendRequest(data);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Cancel friend request failed');
            }
            return response.data;
        },
        onSuccess: (_data, variables) => {
            dispatch(removeFriendPending(variables.friendID));
        },
    });
};

export const useUpdateUserRelationship = (): UseMutationResult<UpdateUserRelationshipResponse, AxiosError<ApiResponse<UpdateUserRelationshipResponse>>, UpdateUserRelationship> => {
    const dispatch = useDispatch();
    const pendingList = useSelector(selectFriendsPending);

    return useMutation({
        mutationFn: async (data: UpdateUserRelationship): Promise<UpdateUserRelationshipResponse> => {
            const response = await updateUserRelationship(data);
            if (!response.isSuccess) {
                throw new Error(response.message || 'Update user relationship failed');
            }
            return response.data;
        },
        onSuccess: (_data, variables) => {
            dispatch(removeFriendPending(variables.friendID));
            if (variables.status === userRelationshipStatus.Accepted) {
                const acceptedFriend = pendingList.find(friend => friend.id === variables.friendID);
                if (acceptedFriend) {
                    const newFriend: Friend = _data as Friend;
                    dispatch(addFriendSlice(newFriend));
                }
            }
        },
    });
};
