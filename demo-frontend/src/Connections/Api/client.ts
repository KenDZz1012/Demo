// src/api/clients.ts
import { createApiClient } from './axiosInstanceFactory';

export const userApi = createApiClient(process.env.REACT_APP_URL_USER!);

