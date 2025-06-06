import axios, { AxiosInstance } from 'axios';
import { useDispatch } from 'react-redux';
import { logout } from '../../features/auth/authSlice';
let isRefreshing = false;
let failedQueue: { resolve: Function; reject: Function }[] = [];

const processQueue = (error: any, token: string | null = null) => {
    failedQueue.forEach(prom => {
        if (error) prom.reject(error);
        else prom.resolve(token);
    });

    failedQueue = [];
};

export const createApiClient = (baseURL: string): AxiosInstance => {
    const instance = axios.create({
        baseURL,
        withCredentials: true,
    });

    instance.interceptors.response.use(
        response => response,
        async error => {
            const originalRequest = error.config;
            const dispatch = useDispatch();
            if (
                error.response?.status === 401 &&
                !originalRequest._retry
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
                    await axios.post(`${process.env.REACT_APP_URL_AUTH}/auth/refresh`, null, {
                        withCredentials: true,
                    });

                    processQueue(null);
                    return instance(originalRequest);
                } catch (err) {
                    processQueue(err, null);
                    window.location.href = '/login'; // chuyển về login
                    dispatch(logout());
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
