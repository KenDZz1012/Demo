import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { AuthService } from '../../core/services/auth.service';
import { AuthStateService } from '../../core/state/auth-state.service';

@Component({
  selector: 'app-user-footer-sidebar',
  standalone: true,
  imports: [AsyncPipe, NzDropDownModule, NzMenuModule, NzIconModule],
  template: `
    <div class="user-bar">
      <div
        class="user-bar__card"
        nz-dropdown
        [nzDropdownMenu]="menu"
        nzTrigger="click"
        nzPlacement="topLeft"
      >
        <div class="kv-avatar user-bar__avatar">
          <img
            class="kv-avatar__img"
            [src]="(authState.user$ | async)?.avatarUrl || '/logo.svg'"
            alt="avatar"
          />
          <span class="kv-avatar__status kv-avatar__status--online"></span>
        </div>
        <div class="user-bar__info">
          <span class="user-bar__name">{{ (authState.user$ | async)?.displayName }}</span>
          <span class="user-bar__status">Online</span>
        </div>
        <span nz-icon nzType="setting" class="user-bar__settings"></span>
      </div>
    </div>

    <nz-dropdown-menu #menu="nzDropdownMenu">
      <ul nz-menu class="menu-server-setting">
        <li nz-menu-item (click)="logout()">
          <span class="user-bar__logout">
            <span>Log out</span>
            <span nz-icon nzType="logout"></span>
          </span>
        </li>
      </ul>
    </nz-dropdown-menu>
  `,
  styles: [`
    .user-bar {
      position: absolute;
      bottom: 12px;
      left: 110px;
      width: 300px;
      z-index: 100;
    }

    .user-bar__card {
      display: flex;
      align-items: center;
      gap: 10px;
      padding: 8px 10px;
      background: var(--kv-bg-elevated);
      border-radius: var(--kv-radius-md);
      cursor: pointer;
      transition: background var(--kv-transition);
    }

    .user-bar__card:hover {
      background: #45454f;
    }

    .user-bar__avatar {
      width: 36px;
      height: 36px;
    }

    .user-bar__info {
      flex: 1;
      min-width: 0;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
    }

    .user-bar__name {
      color: var(--kv-text-primary);
      font-size: 14px;
      font-weight: 600;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      max-width: 180px;
    }

    .user-bar__status {
      color: var(--kv-text-muted);
      font-size: 12px;
    }

    .user-bar__settings {
      color: var(--kv-text-muted);
      font-size: 16px;
    }

    .user-bar__logout {
      display: flex;
      justify-content: space-between;
      align-items: center;
      width: 160px;
      color: var(--kv-error);
    }
  `],
})
export class UserFooterSidebarComponent {
  readonly authState = inject(AuthStateService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  async logout(): Promise<void> {
    try {
      const refreshToken = localStorage.getItem('refreshToken') || '';
      await this.authService.logout(refreshToken);
      await this.router.navigate(['/login'], { replaceUrl: true });
    } catch (error) {
      console.error('Logout failed:', error);
    }
  }
}
