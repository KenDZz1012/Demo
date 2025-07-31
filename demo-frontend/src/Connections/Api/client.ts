import { createApiClient } from './axiosInstanceFactory';

export const createUserApi = (onLogout: () => void) =>
    createApiClient({ baseURL: process.env.REACT_APP_URL_USER!, onLogout });

export const createAuthApi = (onLogout: () => void) =>
    createApiClient({ baseURL: process.env.REACT_APP_URL_AUTH!, onLogout });

export const createChannelApi = (onLogout: () => void) =>
    createApiClient({ baseURL: process.env.REACT_APP_URL_CHANNEL!, onLogout });
