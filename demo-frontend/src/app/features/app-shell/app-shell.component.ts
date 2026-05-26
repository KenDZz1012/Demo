import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterOutlet, ActivatedRoute } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { AuthStateService } from '../../core/state/auth-state.service';
import { ChannelApiService } from '../../core/services/channel-api.service';
import { ServerStateService } from '../../core/state/server-state.service';
import { LoadingScreenComponent } from '../../shared/components/loading-screen/loading-screen.component';
import { ServerSidebarComponent } from '../../layouts/server-sidebar/server-sidebar.component';
import { UserFooterSidebarComponent } from '../../layouts/user-footer-sidebar/user-footer-sidebar.component';
import { CreateServerComponent } from './modals/create-server/create-server.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [
    RouterOutlet,
    NzLayoutModule,
    LoadingScreenComponent,
    ServerSidebarComponent,
    UserFooterSidebarComponent,
    CreateServerComponent,
  ],
  template: `
    @if (loading) {
      <app-loading-screen />
    } @else {
      <nz-layout style="height: 100vh; background-color: #21212a">
        <app-create-server [open]="openCreateServerModal" [ownerId]="ownerId" (closed)="openCreateServerModal = false" />
        <nz-sider [nzWidth]="100" style="padding: 10px; background-color: #21212a">
          <app-server-sidebar
            [servers]="serverState.servers"
            [selectedServerId]="serverState.selectedServerId"
            (selectServer)="onSelectServer($event)"
            (openCreateServer)="openCreateServerModal = true"
          />
        </nz-sider>
        <app-user-footer-sidebar />
        <nz-layout>
          <nz-content style="background-color: #21212a; overflow: auto">
            <router-outlet />
          </nz-content>
        </nz-layout>
      </nz-layout>
    }
  `,
})
export class AppShellComponent implements OnInit {
  readonly authState = inject(AuthStateService);
  readonly serverState = inject(ServerStateService);
  private readonly channelApi = inject(ChannelApiService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  loading = true;
  openCreateServerModal = false;
  ownerId?: string;

  ngOnInit(): void {
    this.ownerId = this.authState.user?.id;
    setTimeout(() => (this.loading = false), 2000);
    this.loadServers();
    this.route.firstChild?.params.subscribe(() => this.syncSelectedServer());
  }

  async loadServers(): Promise<void> {
    if (!this.ownerId) return;
    try {
      const response = await this.channelApi.fetchServers({ ownerId: this.ownerId });
      if (response.isSuccess) {
        this.serverState.setServers(response.data);
      }
    } catch (error) {
      console.error(error);
    }
    this.syncSelectedServer();
  }

  syncSelectedServer(): void {
    const urlServerId = this.route.firstChild?.snapshot.paramMap.get('id') ?? '@me';
    if (this.serverState.selectedServerId !== urlServerId) {
      this.serverState.setSelectedServerId(urlServerId);
    }
    const isValid = urlServerId === '@me' || this.serverState.servers.some((s) => s.id === urlServerId);
    if (!this.loading && !isValid) {
      this.router.navigate(['/server/@me'], { replaceUrl: true });
    }
  }

  onSelectServer(serverId: string): void {
    this.serverState.setSelectedServerId(serverId);
    this.router.navigate(['/server', serverId], { replaceUrl: true });
  }
}
