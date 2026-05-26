import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { CustomInputComponent } from '../../../shared/components/custom-input/custom-input.component';
import { Friend } from '../../../shared/types/user';

@Component({
  selector: 'app-list-friend-sidebar',
  standalone: true,
  imports: [NzIconModule, CustomInputComponent],
  template: `
    <aside class="kv-panel friend-list">
      <div class="friend-list__search">
        <app-custom-input placeholder="Find or start a conversation" [dark]="true" />
      </div>

      <button
        type="button"
        class="friend-list__nav-item"
        [class.friend-list__nav-item--active]="!friendId"
        (click)="selectedFriend.emit(null)"
      >
        <span nz-icon nzType="team" class="friend-list__nav-icon"></span>
        <span>Friends</span>
      </button>

      <div class="friend-list__section">
        <div class="friend-list__section-title">
          <span>Direct Messages</span>
          <span nz-icon nzType="plus" class="friend-list__add"></span>
        </div>

        @for (friend of friends; track friend.id) {
          <button
            type="button"
            class="friend-list__item"
            [class.friend-list__item--active]="friendId === friend.id"
            (click)="selectedFriend.emit(friend)"
          >
            <div class="kv-avatar friend-list__avatar">
              <img class="kv-avatar__img" [src]="friend.avatarUrl || '/logo.svg'" [alt]="friend.displayName" />
              <span
                class="kv-avatar__status"
                [class.kv-avatar__status--online]="friend.isOnline"
                [class.kv-avatar__status--offline]="!friend.isOnline"
              ></span>
            </div>
            <span class="friend-list__name">{{ friend.displayName }}</span>
          </button>
        }
      </div>
    </aside>
  `,
  styles: [`
    .friend-list {
      display: flex;
      flex-direction: column;
      padding-top: 8px;
    }

    .friend-list__search {
      padding: 0 12px 12px;
      border-bottom: 1px solid var(--kv-border-subtle);
    }

    .friend-list__nav-item {
      display: flex;
      align-items: center;
      gap: 12px;
      width: calc(100% - 16px);
      margin: 8px;
      padding: 8px 10px;
      border: none;
      border-radius: var(--kv-radius-md);
      background: transparent;
      color: var(--kv-text-secondary);
      font-size: 15px;
      font-weight: 500;
      cursor: pointer;
      transition: background var(--kv-transition), color var(--kv-transition);
    }

    .friend-list__nav-item:hover,
    .friend-list__nav-item--active {
      background: #3a3c46;
      color: var(--kv-text-primary);
    }

    .friend-list__nav-icon { font-size: 20px; }

    .friend-list__section {
      flex: 1;
      overflow-y: auto;
      padding: 8px 0;
    }

    .friend-list__section-title {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 8px 16px;
      font-size: 11px;
      font-weight: 700;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      color: var(--kv-text-muted);
    }

    .friend-list__add {
      cursor: pointer;
      font-size: 14px;
      transition: color var(--kv-transition);
    }

    .friend-list__add:hover { color: var(--kv-text-primary); }

    .friend-list__item {
      display: flex;
      align-items: center;
      gap: 12px;
      width: calc(100% - 16px);
      margin: 2px 8px;
      padding: 8px 10px;
      border: none;
      border-radius: var(--kv-radius-md);
      background: transparent;
      color: var(--kv-text-secondary);
      cursor: pointer;
      transition: background var(--kv-transition), color var(--kv-transition);
    }

    .friend-list__item:hover,
    .friend-list__item--active {
      background: #45464f;
      color: var(--kv-text-primary);
    }

    .friend-list__avatar { width: 36px; height: 36px; }

    .friend-list__name {
      font-size: 15px;
      font-weight: 500;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
  `],
})
export class ListFriendSidebarComponent {
  @Input() friends: Friend[] = [];
  @Input() friendId?: string | null;
  @Output() selectedFriend = new EventEmitter<Friend | null>();
}
