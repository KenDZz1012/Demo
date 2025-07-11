// src/api/clients.ts
import { createApiClient } from './axiosInstanceFactory';

export const userApi = createApiClient(process.env.REACT_APP_URL_USER!);

export const authApi = createApiClient(process.env.REACT_APP_URL_AUTH!);

export const channelApi = createApiClient(process.env.REACT_APP_URL_CHANNEL!);