import { LoginRequest, TokenResponse } from 'types'
import { ApiResponse } from 'Connections/Api/apiResponse'
import { authApi } from 'Connections/Api/client'

export const login = async (data: LoginRequest): Promise<ApiResponse<TokenResponse>> => {
    return (await authApi.post<ApiResponse<TokenResponse>>('/login', data)).data
}

export const refreshToken = async (refreshToken: string): Promise<ApiResponse<TokenResponse>> => {
    return (await authApi.post<ApiResponse<TokenResponse>>('/refresh', { refreshToken })).data
}
