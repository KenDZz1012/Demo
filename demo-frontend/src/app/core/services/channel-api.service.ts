import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/types/api-response';
import {
  CreateServer,
  JoinServerByInviteLinkRequest,
  LeaveServerRequest,
  Server,
  ServerDetail,
} from '../../shared/types/server';
import { CreateChannel } from '../../shared/types/channel';
import { spreadSearchQuery } from '../../shared/utils/spread-search-query';
import { ServerStateService } from '../state/server-state.service';

@Injectable({ providedIn: 'root' })
export class ChannelApiService {
  private readonly http = inject(HttpClient);
  private readonly serverState = inject(ServerStateService);
  private readonly baseUrl = `${environment.urlChannel}/server`;

  fetchServers(params: Record<string, unknown>): Promise<ApiResponse<Server[]>> {
    return firstValueFrom(
      this.http.get<ApiResponse<Server[]>>(`${this.baseUrl}${spreadSearchQuery(params)}`)
    );
  }

  fetchServerDetail(serverId: string): Promise<ApiResponse<ServerDetail>> {
    return firstValueFrom(
      this.http.get<ApiResponse<ServerDetail>>(`${this.baseUrl}/Detail/${serverId}`)
    );
  }

  fetchServer(serverId: string): Promise<ApiResponse<Server>> {
    return firstValueFrom(
      this.http.get<ApiResponse<Server>>(`${this.baseUrl}/${serverId}`)
    );
  }

  createServer(payload: CreateServer): Promise<ApiResponse<string>> {
    return firstValueFrom(
      this.http.post<ApiResponse<string>>(this.baseUrl, payload)
    );
  }

  deleteServer(serverId: string): Promise<ApiResponse<boolean>> {
    return firstValueFrom(
      this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${serverId}`)
    );
  }

  joinServerByInviteLink(data: JoinServerByInviteLinkRequest): Promise<ApiResponse<string>> {
    return firstValueFrom(
      this.http.post<ApiResponse<string>>(`${this.baseUrl}/JoinServerByInviteLink`, data)
    );
  }

  leaveServer(data: LeaveServerRequest): Promise<ApiResponse<boolean>> {
    return firstValueFrom(
      this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/LeaveServer`, data)
    );
  }

  createChannel(payload: CreateChannel): Promise<ApiResponse<string>> {
    return firstValueFrom(
      this.http.post<ApiResponse<string>>(`${environment.urlChannel}/channel`, payload)
    );
  }

  async createServerAndNavigate(payload: CreateServer): Promise<ServerDetail> {
    const result = await this.createServer(payload);
    if (!result.isSuccess) throw new Error(result.message);
    const detail = await this.fetchServerDetail(result.data);
    if (!detail.isSuccess) throw new Error(detail.message);
    this.serverState.addServer(detail.data);
    this.serverState.setSelectedServer(detail.data);
    this.serverState.setSelectedServerId(detail.data.id);
    return detail.data;
  }

  async joinServerAndNavigate(data: JoinServerByInviteLinkRequest): Promise<Server> {
    const result = await this.joinServerByInviteLink(data);
    if (!result.isSuccess) throw new Error(result.message);
    const detail = await this.fetchServer(result.data);
    if (!detail.isSuccess) throw new Error(detail.message);
    this.serverState.addServer(detail.data);
    return detail.data;
  }
}
