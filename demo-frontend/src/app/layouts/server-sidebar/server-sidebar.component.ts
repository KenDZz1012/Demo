import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Server } from '../../shared/types/server';

@Component({
  selector: 'app-server-sidebar',
  standalone: true,
  imports: [NzIconModule],
  template: `
    <nav class="server-nav menu-hide-scroll">
      <button
        type="button"
        class="server-nav__item server-nav__item--home"
        [class.server-nav__item--active]="selectedServerId === '@me'"
        (click)="onSelectServer('@me')"
        title="Direct Messages"
      >
        @if (selectedServerId === '@me') { <span class="server-nav__pill"></span> }
        <img src="/logo.svg" alt="Home" class="server-nav__home-logo" />
      </button>

      <span class="server-nav__divider"></span>

      @for (server of servers; track server.id) {
        <button
          type="button"
          class="server-nav__item"
          [class.server-nav__item--active]="selectedServerId === server.id"
          (click)="onSelectServer(server.id)"
          [title]="server.name"
        >
          @if (selectedServerId === server.id) { <span class="server-nav__pill"></span> }
          @if (server.iconUrl) {
            <img [src]="server.iconUrl" [alt]="server.name" class="server-nav__icon" />
          } @else {
            <span class="server-nav__letter">{{ server.name[0] }}</span>
          }
        </button>
      }

      <button type="button" class="server-nav__item server-nav__item--add" title="Add a Server" (click)="openCreateServer.emit()">
        <span nz-icon nzType="plus" class="server-nav__add-icon"></span>
      </button>
    </nav>
  `,
  styles: [`
    :host {
      display: block;
      height: 100%;
    }

    .server-nav {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 10px;
      padding: 8px 0 120px;
      height: 100%;
      overflow-y: auto;
    }

    .server-nav__item {
      position: relative;
      width: 48px;
      height: 48px;
      margin-left: 0;
      align-self: center;
      border: none;
      border-radius: 16px;
      background: var(--kv-bg-server);
      color: #fff;
      cursor: pointer;
      display: grid;
      place-items: center;
      transition: border-radius var(--kv-transition), background var(--kv-transition), transform 0.15s ease;
      overflow: visible;
    }

    .server-nav__item:hover {
      border-radius: 12px;
      background: var(--kv-blurple);
    }

    .server-nav__item--active {
      border-radius: 12px;
    }

    .server-nav__item--add:hover {
      background: var(--kv-success);
    }

    .server-nav__pill {
      position: absolute;
      left: -16px;
      top: 50%;
      transform: translateY(-50%);
      width: 4px;
      height: 20px;
      background: #fff;
      border-radius: 0 4px 4px 0;
      animation: growLine 0.25s ease-out forwards;
    }

    .server-nav__divider {
      width: 32px;
      height: 2px;
      background: var(--kv-border-subtle);
      border-radius: 1px;
    }

    .server-nav__home-logo {
      width: 28px;
      height: 28px;
      object-fit: contain;
    }

    .server-nav__icon {
      width: 100%;
      height: 100%;
      object-fit: cover;
      border-radius: inherit;
    }

    .server-nav__letter {
      font-size: 18px;
      font-weight: 700;
    }

    .server-nav__add-icon {
      font-size: 22px;
      transition: transform 0.2s ease;
    }

    .server-nav__item--add:hover .server-nav__add-icon {
      transform: rotate(90deg);
    }
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
