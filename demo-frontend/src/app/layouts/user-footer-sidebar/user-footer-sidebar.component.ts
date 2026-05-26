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
    <div style="position: absolute; bottom: 10px; width: 390px; padding-left: 10px; z-index: 1000">
      <div style="position: sticky; bottom: 0; padding: 8px; background-color: #3b3b47; border-radius: 10px; display: flex">
        <div nz-dropdown [nzDropdownMenu]="menu" nzTrigger="click" nzPlacement="bottomLeft"
             style="display: flex; align-items: center; gap: 10px; width: 80%; padding: 6px 0; border-radius: 10px; cursor: pointer"
             (mouseenter)="hovered = true" (mouseleave)="hovered = false"
             [style.backgroundColor]="hovered ? '#41414b' : 'transparent'">
          <div style="position: relative; width: 36px; height: 36px">
            <img [src]="(authState.user$ | async)?.avatarUrl || '/logo.svg'" alt="avatar"
                 style="width: 100%; height: 100%; border-radius: 50%; object-fit: cover; background-color: #6b6967" />
            <span style="position: absolute; bottom: 0; right: 0; width: 10px; height: 10px; background-color: green; border-radius: 50%; border: 2px solid white"></span>
          </div>
          <div style="display: flex; flex-direction: column; align-items: flex-start">
            <span style="color: white; font-size: 16px">{{ (authState.user$ | async)?.displayName }}</span>
            <span style="color: white; font-size: 12px">Online</span>
          </div>
        </div>
      </div>
    </div>

    <nz-dropdown-menu #menu="nzDropdownMenu">
      <ul nz-menu class="menu-server-setting" style="background-color: #001529">
        <li nz-menu-item (click)="logout()">
          <div style="display: flex; justify-content: space-between; color: #f17875">
            <span>Log out</span>
            <span nz-icon nzType="logout"></span>
          </div>
        </li>
      </ul>
    </nz-dropdown-menu>
  `,
})
export class UserFooterSidebarComponent {
  readonly authState = inject(AuthStateService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  hovered = false;

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
