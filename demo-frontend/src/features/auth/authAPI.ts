import { LoginRequest, LogoutRequest, TokenResponse } from 'types'
import { ApiResponse } from 'types/apiResponse'
import { authApi } from 'Connections/Api/useAPIClient'


const login = async (data: LoginRequest): Promise<ApiResponse<TokenResponse>> => {
    return (await authApi.post<ApiResponse<TokenResponse>>('/login', data)).data
}

const logout = async (data: LogoutRequest): Promise<ApiResponse<boolean>> => {
    return (await authApi.post<ApiResponse<boolean>>('/logout', data)).data
}

export {
    login,
    logout
}