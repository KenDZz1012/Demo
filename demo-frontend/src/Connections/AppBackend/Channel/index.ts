import { useQuery, UseQueryResult } from '@tanstack/react-query';
import { channelApi } from '../../Api/client';
import { ApiResponse } from '../../Api/apiResponse';
import { Server } from '../../Types/Channel';

export const useChannels = (): UseQueryResult<ApiResponse<Server[]>, Error> =>
    useQuery({
        queryKey: ['channels'],
        queryFn: () => channelApi.get("/server").then(r => r.data),
    });