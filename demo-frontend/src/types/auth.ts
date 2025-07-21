export interface LoginRequest {
    userName: string;
    password: string;
}

export interface TokenResponse {
    accessToken: string;
    refreshToken: string;
    userID: string;
}

export interface AuthUser {
    id: string;
    username: string;
    email: string;
    avatarUrl?: string;
}

export interface AuthState {
    isAuthenticated: boolean;
    accessToken: string | null;
    refreshToken?: string | null;
    user: AuthUser | null;
}