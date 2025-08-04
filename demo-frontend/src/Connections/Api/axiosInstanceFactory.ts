import axios, { AxiosInstance, AxiosError } from 'axios';
import { useSelector } from 'react-redux';
import { startSignalRConnection, stopSignalRConnection } from 'signalr/signalrConnection';
import { selectAuthUser } from 'store/selectors/authSelectors';

let isRefreshing = false;
let failedQueue: { resolve: Function; reject: Function }[] = [];



const processQueue = (error: any, token: string | null = null) => {
    failedQueue.forEach(prom => {
        if (error) prom.reject(error);
        else prom.resolve(token);
    });
    failedQueue = [];
};

interface CreateApiClientOptions {
    baseURL: string;
    onLogout: () => void; // Inject từ component React
}

export const createApiClient = ({ baseURL, onLogout }: CreateApiClientOptions): AxiosInstance => {
    const instance = axios.create({ baseURL });
    const authUser = JSON.parse(localStorage.getItem('user') || 'null');
    instance.interceptors.request.use(
        (config) => {
            const token = localStorage.getItem('token');
            if (token) {
                config.headers = config.headers || {};
                config.headers['Authorization'] = `Bearer ${token}`;
            }
            return config;
        },
        (error) => Promise.reject(error)
    );

    instance.interceptors.response.use(
        response => response,
        async (error: AxiosError & { config: any }) => {
            const originalRequest = error.config;

            if (
                error.response?.status === 401 &&
                !originalRequest._retry &&
                !originalRequest.url.includes('/login') &&
                !originalRequest.url.includes('/auth/refresh')
            ) {
                originalRequest._retry = true;

                if (isRefreshing) {
                    return new Promise((resolve, reject) => {
                        failedQueue.push({
                            resolve: (token: string) => resolve(instance(originalRequest)),
                            reject: (err: any) => reject(err),
                        });
                    });
                }

                isRefreshing = true;

                try {
                    const refreshToken = localStorage.getItem('refreshToken');
                    const refreshResponse = await axios.post(
                        `${process.env.REACT_APP_URL_AUTH}/refresh`,
                        { refreshToken, userId: authUser?.id }
                    );

                    const { accessToken, refreshToken: newRefreshToken } = refreshResponse.data.data;
                    if (accessToken) localStorage.setItem('token', accessToken);
                    if (newRefreshToken) localStorage.setItem('refreshToken', newRefreshToken);
                    await stopSignalRConnection();
                    await startSignalRConnection();
                    processQueue(null, accessToken);
                    originalRequest.headers['Authorization'] = `Bearer ${accessToken}`;

                    return instance(originalRequest);
                } catch (err) {
                    processQueue(err, null);
                    onLogout();
                    return Promise.reject(err);
                } finally {
                    isRefreshing = false;
                }
            }

            return Promise.reject(error);
        }
    );

    return instance;
};
