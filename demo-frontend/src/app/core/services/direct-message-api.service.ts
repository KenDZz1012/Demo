import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../shared/types/api-response';
import { SendMessageRequest } from '../../shared/types/direct-message';

@Injectable({ providedIn: 'root' })
export class DirectMessageApiService {
  private readonly http = inject(HttpClient);

  sendMessage(data: SendMessageRequest): Promise<ApiResponse<string>> {
    return firstValueFrom(
      this.http.post<ApiResponse<string>>(`${environment.urlDirectMessage}/DirectMessage/SendMessage`, data)
    );
  }
}
