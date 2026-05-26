import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/types/api-response';
import {
  AddFriendRequest,
  CancelFriendRequest,
  CreateUserRelationshipResponse,
  Friend,
  FriendPending,
  UpdateUserRelationship,
  UpdateUserRelationshipResponse,
} from '../../shared/types/user';
import { spreadSearchQuery } from '../../shared/utils/spread-search-query';

@Injectable({ providedIn: 'root' })
export class UserRelationshipApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.urlUser}/UserRelationship`;

  fetchFriends(params: Record<string, unknown>): Promise<ApiResponse<Friend[]>> {
    return firstValueFrom(
      this.http.get<ApiResponse<Friend[]>>(`${this.baseUrl}/Friends${spreadSearchQuery(params)}`)
    );
  }

  fetchFriendsPending(params: Record<string, unknown>): Promise<ApiResponse<FriendPending[]>> {
    return firstValueFrom(
      this.http.get<ApiResponse<FriendPending[]>>(`${this.baseUrl}/FriendsPending${spreadSearchQuery(params)}`)
    );
  }

  addFriend(data: AddFriendRequest): Promise<ApiResponse<CreateUserRelationshipResponse>> {
    return firstValueFrom(
      this.http.post<ApiResponse<CreateUserRelationshipResponse>>(this.baseUrl, data)
    );
  }

  cancelFriendRequest(data: CancelFriendRequest): Promise<ApiResponse<string>> {
    return firstValueFrom(
      this.http.put<ApiResponse<string>>(`${this.baseUrl}/Delete`, data)
    );
  }

  updateUserRelationship(data: UpdateUserRelationship): Promise<ApiResponse<UpdateUserRelationshipResponse>> {
    return firstValueFrom(
      this.http.put<ApiResponse<UpdateUserRelationshipResponse>>(this.baseUrl, data)
    );
  }
}
