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
      <div class="app-shell">
        <app-create-server [open]="openCreateServerModal" [ownerId]="ownerId" (closed)="openCreateServerModal = false" />

        <aside class="app-shell__servers">
          <app-server-sidebar
            [servers]="serverState.servers"
            [selectedServerId]="serverState.selectedServerId"
            (selectServer)="onSelectServer($event)"
            (openCreateServer)="openCreateServerModal = true"
          />
        </aside>

        <main class="app-shell__main">
          <router-outlet />
        </main>

        <app-user-footer-sidebar />
      </div>
    }
  `,
  styles: [`
    .app-shell {
      display: grid;
      grid-template-columns: 72px 1fr;
      height: 100vh;
      background: var(--kv-bg-primary);
      position: relative;
    }

    .app-shell__servers {
      padding: 12px 8px;
      background: var(--kv-bg-primary);
      border-right: 1px solid var(--kv-border-subtle);
    }

    .app-shell__main {
      overflow: hidden;
      padding: 10px 10px 10px 0;
    }
  `],
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
    setTimeout(() => (this.loading = false), 1500);
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
