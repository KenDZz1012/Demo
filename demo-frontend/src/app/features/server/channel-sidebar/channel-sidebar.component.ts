import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Channel } from '../../../shared/types/channel';

@Component({
  selector: 'app-channel-sidebar',
  standalone: true,
  imports: [NzDropDownModule, NzMenuModule, NzIconModule],
  template: `
    <aside class="kv-panel channel-sidebar">
      <div
        class="channel-sidebar__header"
        nz-dropdown
        [nzDropdownMenu]="menu"
        nzTrigger="click"
        nzPlacement="bottomLeft"
      >
        <span class="channel-sidebar__name">{{ serverName }}</span>
        <span nz-icon nzType="down"></span>
      </div>

      <div class="channel-sidebar__section">
        <div class="channel-sidebar__section-head">
          <span>Text Channels</span>
          <button type="button" class="channel-sidebar__add" (click)="addChannel.emit()">
            <span nz-icon nzType="plus"></span>
          </button>
        </div>
        @for (channel of textChannels; track channel.id) {
          <button
            type="button"
            class="channel-sidebar__channel"
            [class.channel-sidebar__channel--active]="selectedChannelId === channel.id"
            (click)="selectChannel.emit(channel.id)"
          >
            <span class="channel-sidebar__hash">#</span>
            {{ channel.name }}
          </button>
        }
      </div>

      <div class="channel-sidebar__section">
        <div class="channel-sidebar__section-head">
          <span>Voice Channels</span>
          <button type="button" class="channel-sidebar__add" (click)="addChannel.emit()">
            <span nz-icon nzType="plus"></span>
          </button>
        </div>
        @for (channel of voiceChannels; track channel.id) {
          <button type="button" class="channel-sidebar__channel channel-sidebar__channel--voice">
            <span nz-icon nzType="sound"></span>
            {{ channel.name }}
          </button>
        }
      </div>
    </aside>

    <nz-dropdown-menu #menu="nzDropdownMenu">
      <ul nz-menu class="menu-server-setting">
        <li nz-menu-item (click)="addChannel.emit()">
          <span class="channel-sidebar__menu-item"><span>Create Channel</span><span nz-icon nzType="plus-circle"></span></span>
        </li>
        <li nz-menu-item (click)="invitePeople.emit()">
          <span class="channel-sidebar__menu-item"><span>Invite People</span><span nz-icon nzType="usergroup-add"></span></span>
        </li>
        @if (isOwner) {
          <li nz-menu-item (click)="deleteServer.emit()">
            <span class="channel-sidebar__menu-item channel-sidebar__menu-item--danger"><span>Delete Server</span><span nz-icon nzType="delete"></span></span>
          </li>
        } @else {
          <li nz-menu-item (click)="leaveServer.emit()">
            <span class="channel-sidebar__menu-item channel-sidebar__menu-item--danger"><span>Leave Server</span><span nz-icon nzType="export"></span></span>
          </li>
        }
      </ul>
    </nz-dropdown-menu>
  `,
  styles: [`
    .channel-sidebar {
      display: flex;
      flex-direction: column;
      padding-top: 0;
    }

    .channel-sidebar__header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px 18px;
      border-bottom: 1px solid var(--kv-border-subtle);
      color: var(--kv-text-primary);
      font-weight: 700;
      cursor: pointer;
      transition: background var(--kv-transition);
    }

    .channel-sidebar__header:hover { background: rgba(255, 255, 255, 0.04); }

    .channel-sidebar__name {
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }

    .channel-sidebar__section { padding: 12px 8px; }

    .channel-sidebar__section-head {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 4px 10px 8px;
      font-size: 11px;
      font-weight: 700;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      color: var(--kv-text-muted);
    }

    .channel-sidebar__add {
      background: none;
      border: none;
      color: var(--kv-text-muted);
      cursor: pointer;
      padding: 2px;
      border-radius: 4px;
      transition: color var(--kv-transition), background var(--kv-transition);
    }

    .channel-sidebar__add:hover {
      color: var(--kv-text-primary);
      background: rgba(255, 255, 255, 0.08);
    }

    .channel-sidebar__channel {
      display: flex;
      align-items: center;
      gap: 6px;
      width: 100%;
      padding: 8px 10px;
      border: none;
      border-radius: var(--kv-radius-md);
      background: transparent;
      color: var(--kv-text-secondary);
      font-size: 15px;
      cursor: pointer;
      transition: background var(--kv-transition), color var(--kv-transition);
    }

    .channel-sidebar__channel:hover,
    .channel-sidebar__channel--active {
      background: #45464f;
      color: var(--kv-text-primary);
    }

    .channel-sidebar__hash {
      font-size: 18px;
      font-weight: 600;
      opacity: 0.7;
    }

    .channel-sidebar__channel--voice span[nz-icon] {
      font-size: 16px;
      opacity: 0.7;
    }

    .channel-sidebar__menu-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 180px;
    }

    .channel-sidebar__menu-item--danger { color: var(--kv-error); }
  `],
})
export class ChannelSidebarComponent {
  @Input() channels: Channel[] = [];
  @Input() serverName = '';
  @Input() isOwner = false;
  @Input() selectedChannelId: string | null = null;
  @Output() selectChannel = new EventEmitter<string>();
  @Output() addChannel = new EventEmitter<void>();
  @Output() deleteServer = new EventEmitter<void>();
  @Output() leaveServer = new EventEmitter<void>();
  @Output() invitePeople = new EventEmitter<void>();

  get textChannels(): Channel[] {
    return this.channels.filter((c) => c.type === 'text');
  }

  get voiceChannels(): Channel[] {
    return this.channels.filter((c) => c.type === 'voice');
  }
}
