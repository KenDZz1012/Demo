import { AuthUser } from "./user";

export interface LoginRequest {
    userName: string;
    password: string;
}

export interface TokenResponse {
    accessToken: string;
    refreshToken: string;
    user: AuthUser;
}

export interface AuthState {
    isAuthenticated: boolean;
    accessToken: string | null;
    refreshToken?: string | null;
    user: AuthUser | null;
}