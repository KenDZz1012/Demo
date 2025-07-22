import { LoginRequest, TokenResponse } from 'types'
import { ApiResponse } from 'types/apiResponse'
import { authApi } from 'Connections/Api/client'

const login = async (data: LoginRequest): Promise<ApiResponse<TokenResponse>> => {
    return (await authApi.post<ApiResponse<TokenResponse>>('/login', data)).data
}

const refreshToken = async (refreshToken: string): Promise<ApiResponse<TokenResponse>> => {
    return (await authApi.post<ApiResponse<TokenResponse>>('/refresh', { refreshToken })).data
}

export {
    login,
    refreshToken
}