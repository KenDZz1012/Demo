import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Channel } from '../../shared/types/channel';
import { Server, ServerDetail } from '../../shared/types/server';

@Injectable({ providedIn: 'root' })
export class ServerStateService {
  private readonly serversSubject = new BehaviorSubject<Server[]>([]);
  private readonly selectedServerSubject = new BehaviorSubject<ServerDetail | null>(null);
  private readonly selectedServerIdSubject = new BehaviorSubject<string | null>(null);

  readonly servers$ = this.serversSubject.asObservable();
  readonly selectedServer$ = this.selectedServerSubject.asObservable();
  readonly selectedServerId$ = this.selectedServerIdSubject.asObservable();

  get servers(): Server[] {
    return this.serversSubject.value;
  }

  get selectedServer(): ServerDetail | null {
    return this.selectedServerSubject.value;
  }

  get selectedServerId(): string | null {
    return this.selectedServerIdSubject.value;
  }

  setServers(servers: Server[]): void {
    this.serversSubject.next(servers);
  }

  setSelectedServer(server: ServerDetail | null): void {
    this.selectedServerSubject.next(server);
  }

  setSelectedServerId(id: string | null): void {
    this.selectedServerIdSubject.next(id);
  }

  addServer(server: Server): void {
    const exists = this.servers.some((s) => s.id === server.id);
    if (!exists) {
      this.serversSubject.next([...this.servers, server]);
    }
  }

  removeServer(serverId: string): void {
    this.serversSubject.next(this.servers.filter((s) => s.id !== serverId));
  }

  addChannel(channel: Channel): void {
    const server = this.selectedServer;
    if (!server) return;
    const exists = server.channels.some((c) => c.id === channel.id);
    if (exists) return;
    this.selectedServerSubject.next({
      ...server,
      channels: [...server.channels, channel],
    });
  }
}
