import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterOutlet, ActivatedRoute } from '@angular/router';
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
        <aside class="app-shell__servers">
          <app-server-sidebar
            [servers]="serverState.servers"
            [selectedServerId]="serverState.selectedServerId"
            (selectServer)="onSelectServer($event)"
            (openCreateServer)="openCreateServerModal = true"
          />
        </aside>

        <section class="app-shell__workspace">
          <main class="app-shell__main">
            <router-outlet />
          </main>
          <app-user-footer-sidebar />
        </section>

        <app-create-server
          [open]="openCreateServerModal"
          [ownerId]="ownerId"
          (closed)="openCreateServerModal = false"
        />
      </div>
    }
  `,
  styles: [`
    :host {
      display: block;
      height: 100vh;
    }

    .app-shell {
      display: flex;
      height: 100vh;
      overflow: hidden;
      background: var(--kv-bg-primary);
    }

    .app-shell__servers {
      flex: 0 0 72px;
      width: 72px;
      padding: 12px 8px;
      background: var(--kv-bg-primary);
      border-right: 1px solid var(--kv-border-subtle);
      overflow: hidden;
    }

    .app-shell__workspace {
      flex: 1;
      min-width: 0;
      display: flex;
      flex-direction: column;
      position: relative;
      overflow: hidden;
    }

    .app-shell__main {
      flex: 1;
      min-height: 0;
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
