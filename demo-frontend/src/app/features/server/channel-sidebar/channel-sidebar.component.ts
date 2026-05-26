import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Channel } from '../../../shared/types/channel';

@Component({
  selector: 'app-channel-sidebar',
  standalone: true,
  imports: [NzMenuModule, NzDropDownModule, NzIconModule],
  template: `
    <ul nz-menu nzTheme="dark" nzMode="inline" class="channel-menu sidebar">
      <li nz-submenu [nzTitle]="serverName" nz-dropdown [nzDropdownMenu]="menu">
        <ul nz-menu nzSelectable></ul>
      </li>

      <li nz-menu-item nzDisabled class="section-title">
        <div style="display: flex; justify-content: space-between">
          <span>Text Channels</span>
          <span nz-icon nzType="plus" (click)="addChannel.emit()" style="cursor: pointer"></span>
        </div>
      </li>
      @for (channel of textChannels; track channel.id) {
        <li nz-menu-item (click)="selectChannel.emit(channel.id)"># {{ channel.name }}</li>
      }

      <li nz-menu-item nzDisabled class="section-title">
        <div style="display: flex; justify-content: space-between">
          <span>Voice Channels</span>
          <span nz-icon nzType="plus" (click)="addChannel.emit()" style="cursor: pointer"></span>
        </div>
      </li>
      @for (channel of voiceChannels; track channel.id) {
        <li nz-menu-item><span nz-icon nzType="sound"></span> {{ channel.name }}</li>
      }
    </ul>

    <nz-dropdown-menu #menu="nzDropdownMenu">
      <ul nz-menu class="menu-server-setting">
        <li nz-menu-item (click)="addChannel.emit()">Create Channel</li>
        <li nz-menu-item (click)="invitePeople.emit()">Invite People</li>
        @if (isOwner) {
          <li nz-menu-item (click)="deleteServer.emit()"><span style="color: #f17875">Delete Server</span></li>
        } @else {
          <li nz-menu-item (click)="leaveServer.emit()"><span style="color: #f17875">Leave Server</span></li>
        }
      </ul>
    </nz-dropdown-menu>
  `,
  styles: [`
    .sidebar { background-color: #2a2c35; border-top-left-radius: 20px; border-bottom-left-radius: 20px; }
    .section-title { color: #888; font-weight: bold; }
  `],
})
export class ChannelSidebarComponent {
  @Input() channels: Channel[] = [];
  @Input() serverName = '';
  @Input() isOwner = false;
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
