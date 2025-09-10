import { useMutation, UseMutationResult, useQuery, useQueryClient, UseQueryResult } from "@tanstack/react-query";
import { useDispatch, useSelector } from "react-redux";
import { AxiosError } from "axios";
import { userRelationshipStatus } from "shared";
import { addFriend, cancelFriendRequest, fetchFriends, fetchFriendsPending, updateUserRelationship } from "features/user-relationship/userRelationshipAPI";
import { addFriend as addFriendSlice, addFriendPending, removeFriendPending, setFriends, setFriendsPending } from "features/user-relationship/userRelationshipSlice";
import { ApiResponse } from "types/apiResponse";
import { AddFriendRequest, CancelFriendRequest, CreateUserRelationshipResponse, Friend, FriendPending, UpdateUserRelationship, UpdateUserRelationshipResponse } from "types/user";
import { SendMessageRequest } from "types";
import { sendMessage } from "features/direct-message/directMessageAPI";

export const useSendMessage = (): UseMutationResult<string, AxiosError<ApiResponse<string>>, SendMessageRequest> => {
    const dispatch = useDispatch();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (newUser: SendMessageRequest): Promise<string> => {
            const response = await sendMessage(newUser);
            if (!response.isSuccess) {
                throw new AxiosError(response.message || 'Create user failed');
            }
            return response.data;
        },
        onSuccess: (data, variables) => {

        }
    });
};
