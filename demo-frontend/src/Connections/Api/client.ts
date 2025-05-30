// src/api/clients.ts
import { create } from 'domain';
import { createApiClient } from './axiosInstanceFactory';

export const userApi = createApiClient(process.env.REACT_APP_URL_USER!);

export const authApi = createApiClient(process.env.REACT_APP_URL_AUTH!);
