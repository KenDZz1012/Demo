import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Server } from '../../shared/types/server';

@Component({
  selector: 'app-server-sidebar',
  standalone: true,
  imports: [NzIconModule],
  template: `
    <div class="menu-hide-scroll server-menu">
      <button type="button" class="server-item logo-item" [class.selected]="selectedServerId === '@me'" (click)="onSelectServer('@me')" title="Direct Messages">
        @if (selectedServerId === '@me') { <div class="grow-indicator"></div> }
        <img src="/logo.svg" alt="logo" style="width: 32px; height: 40px; object-fit: contain" />
      </button>
      <div class="divider"></div>
      @for (server of servers; track server.id) {
        <button type="button" class="server-item" [class.selected]="selectedServerId === server.id" (click)="onSelectServer(server.id)" [title]="server.name">
          @if (selectedServerId === server.id) { <div class="grow-indicator"></div> }
          @if (server.iconUrl) {
            <img [src]="server.iconUrl" [alt]="server.name" class="server-icon" />
          } @else {
            <p class="server-letter">{{ server.name[0] }}</p>
          }
        </button>
      }
      <button type="button" class="server-item add-server" title="Add a Server" (click)="openCreateServer.emit()">
        <span nz-icon nzType="plus-circle" nzTheme="fill" style="font-size: 20px"></span>
      </button>
    </div>
  `,
  styles: [`
    .server-menu {
      padding: 4px;
      background-color: #2a2c35;
      color: white;
      border-radius: 20px;
      overflow-y: auto;
      overflow-x: hidden;
      padding-bottom: 200px;
      border: none;
      display: flex;
      flex-direction: column;
      align-items: center;
    }
    .server-item {
      background-color: rgb(0, 21, 41);
      border-radius: 16px;
      width: 46px;
      height: 46px;
      margin: 10px 0 0 14px;
      position: relative;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      border: none;
      align-self: flex-start;
    }
    .logo-item { margin-top: 0; }
    .divider { margin-top: 6px; border-bottom: 1px solid #555; width: 50%; align-self: center; }
    .grow-indicator {
      position: absolute;
      left: -12px;
      top: 50%;
      transform: translateY(-50%);
      width: 4px;
      height: 40px;
      border-radius: 4px;
      background-color: #fff;
      z-index: 2;
    }
    .server-icon { width: 100%; height: 100%; object-fit: cover; border-radius: 16px; }
    .server-letter { font-size: 18px; color: #fff; margin: 0; }
  `],
})
export class ServerSidebarComponent {
  @Input() servers: Server[] = [];
  @Input() selectedServerId: string | null = null;
  @Output() selectServer = new EventEmitter<string>();
  @Output() openCreateServer = new EventEmitter<void>();

  onSelectServer(id: string): void {
    this.selectServer.emit(id);
  }
}
