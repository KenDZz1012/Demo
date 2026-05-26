import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/types/api-response';
import { LoginRequest, LogoutRequest, TokenResponse } from '../../shared/types/auth';
import { CreateUserInput } from '../../shared/types/user';
import { AuthStateService } from '../state/auth-state.service';
import { SignalRService } from './signalr.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);
  private readonly signalR = inject(SignalRService);

  saveAuthDataToLocalStorage(data: TokenResponse): void {
    localStorage.setItem('token', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('user', JSON.stringify(data.user));
  }

  removeAuthDataFromLocalStorage(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
  }

  async login(input: LoginRequest): Promise<TokenResponse> {
    const response = await firstValueFrom(
      this.http.post<ApiResponse<TokenResponse>>(`${environment.urlAuth}/login`, input)
    );
    if (!response.isSuccess) {
      throw new Error(response.message || 'Login failed');
    }
    this.saveAuthDataToLocalStorage(response.data);
    this.authState.loginSuccess(response.data.user);
    await this.signalR.startConnection();
    return response.data;
  }

  async logout(refreshToken: string): Promise<boolean> {
    const response = await firstValueFrom(
      this.http.post<ApiResponse<boolean>>(`${environment.urlAuth}/logout`, { refreshToken } satisfies LogoutRequest)
    );
    if (!response.isSuccess) {
      throw new Error(response.message || 'Logout failed');
    }
    await this.signalR.stopConnection();
    this.removeAuthDataFromLocalStorage();
    this.authState.logoutSuccess();
    return response.data;
  }

  async register(input: CreateUserInput): Promise<string> {
    const response = await firstValueFrom(
      this.http.post<ApiResponse<string>>(`${environment.urlUser}/user`, input)
    );
    if (!response.isSuccess) {
      throw new Error(response.message || 'Create user failed');
    }
    return response.data;
  }

  handleForcedLogout(): void {
    this.removeAuthDataFromLocalStorage();
    this.authState.logoutSuccess();
    window.location.href = '/login';
  }
}
