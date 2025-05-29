import axios from 'axios';

export const createApiClient = (baseURL: string) => {
    return axios.create({
        baseURL,
        headers: {
            'Content-Type': 'application/json',
        },
        withCredentials: true, // Nếu dùng cookie hoặc session
    });
};
