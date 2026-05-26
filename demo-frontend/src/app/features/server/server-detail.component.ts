import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthStateService } from '../../core/state/auth-state.service';
import { ChannelApiService } from '../../core/services/channel-api.service';
import { ServerStateService } from '../../core/state/server-state.service';
import { Channel, CreateChannel } from '../../shared/types/channel';
import { ChannelSidebarComponent } from './channel-sidebar/channel-sidebar.component';
import { ServerChatAreaComponent } from './chat-area/server-chat-area.component';
import { CreateChannelComponent } from './modals/create-channel/create-channel.component';
import { InvitePeopleComponent } from './modals/invite-people/invite-people.component';

@Component({
  selector: 'app-server-detail',
  standalone: true,
  imports: [ChannelSidebarComponent, ServerChatAreaComponent, CreateChannelComponent, InvitePeopleComponent],
  template: `
    <div class="server-layout">
      <app-create-channel [visible]="modalVisible" (cancelled)="modalVisible = false" (created)="onCreateChannel($event)" />
      <app-invite-people [visible]="inviteVisible" [server]="serverState.selectedServer" (cancelled)="inviteVisible = false" />

      <app-channel-sidebar
        class="server-layout__sidebar"
        [channels]="serverState.selectedServer?.channels || []"
        [serverName]="serverState.selectedServer?.name || ''"
        [isOwner]="serverState.selectedServer?.ownerId === userId"
        [selectedChannelId]="selectedChannel?.id || null"
        (selectChannel)="handleChannelSelect($event)"
        (addChannel)="modalVisible = true"
        (deleteServer)="deleteServer()"
        (leaveServer)="leaveServer()"
        (invitePeople)="inviteVisible = true"
      />

      <app-server-chat-area
        class="server-layout__chat"
        [channelName]="selectedChannel?.name"
        [messages]="messages"
        [(inputValue)]="input"
        (send)="sendMessage()"
      />
    </div>
  `,
  styles: [`
    .server-layout {
      display: grid;
      grid-template-columns: 300px 1fr;
      height: 100%;
    }

    .server-layout__sidebar,
    .server-layout__chat {
      min-height: 0;
      height: 100%;
    }
  `],
})
export class ServerDetailComponent implements OnInit {
  readonly authState = inject(AuthStateService);
  readonly serverState = inject(ServerStateService);
  private readonly channelApi = inject(ChannelApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  selectedChannel: Channel | null = null;
  messages: string[] = [];
  input = '';
  modalVisible = false;
  inviteVisible = false;
  userId?: string;

  ngOnInit(): void {
    this.userId = this.authState.user?.id;
    this.route.paramMap.subscribe(async (params) => {
      const id = params.get('id');
      if (!id) return;
      try {
        const response = await this.channelApi.fetchServerDetail(id);
        if (!response.isSuccess) {
          await this.router.navigate(['/server/@me'], { replaceUrl: true });
          return;
        }
        this.serverState.setSelectedServer(response.data);
        this.selectedChannel = response.data.channels?.[0] || null;
      } catch {
        await this.router.navigate(['/server/@me'], { replaceUrl: true });
      }
    });
  }

  handleChannelSelect(channelId: string): void {
    const channel = this.serverState.selectedServer?.channels.find((c) => c.id === channelId);
    if (channel) {
      this.selectedChannel = channel;
      this.messages = [];
    }
  }

  sendMessage(): void {
    if (this.input.trim()) {
      this.messages = [...this.messages, this.input];
      this.input = '';
    }
  }

  async deleteServer(): Promise<void> {
    const serverId = this.serverState.selectedServerId;
    if (!serverId) return;
    const response = await this.channelApi.deleteServer(serverId);
    if (response.isSuccess) this.serverState.removeServer(serverId);
  }

  async leaveServer(): Promise<void> {
    const serverId = this.serverState.selectedServerId;
    if (!serverId || !this.userId) return;
    const response = await this.channelApi.leaveServer({ ServerId: serverId, UserId: this.userId });
    if (response.isSuccess) this.serverState.removeServer(serverId);
  }

  async onCreateChannel(input: CreateChannel): Promise<void> {
    const serverId = this.serverState.selectedServerId;
    if (!serverId) return;
    input.serverId = serverId;
    const response = await this.channelApi.createChannel(input);
    if (response.isSuccess) {
      this.serverState.addChannel({ id: response.data, name: input.name, type: input.type });
      this.modalVisible = false;
    }
  }
}
