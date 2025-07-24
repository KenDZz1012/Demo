import { useQuery, UseQueryResult } from "@tanstack/react-query";
import { fetchFriends } from "features/user-relationship/userRelationshipAPI";
import { ApiResponse } from "types/apiResponse";
import { Friend } from "types/user";

export const useFriends = (params: any): UseQueryResult<ApiResponse<Friend[]>, Error> =>
    useQuery({
        queryKey: ['friends', params],
        queryFn: () => fetchFriends(params),
    });
