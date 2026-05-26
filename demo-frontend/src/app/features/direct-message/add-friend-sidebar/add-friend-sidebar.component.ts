import { Component, Input, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { AuthStateService } from '../../../core/state/auth-state.service';
import { UserRelationshipApiService } from '../../../core/services/user-relationship-api.service';
import { UserRelationshipStateService } from '../../../core/state/user-relationship-state.service';
import { userRelationshipStatus } from '../../../shared/constants/user-relationship-status';
import { CustomInputComponent } from '../../../shared/components/custom-input/custom-input.component';
import { CustomButtonComponent } from '../../../shared/components/custom-button/custom-button.component';
import { FriendPending } from '../../../shared/types/user';

@Component({
  selector: 'app-add-friend-sidebar',
  standalone: true,
  imports: [FormsModule, NzIconModule, CustomInputComponent, CustomButtonComponent],
  template: `
    <div class="kv-panel kv-panel--right friends-panel">
      <header class="friends-panel__header">
        <span nz-icon nzType="team"></span>
        <span>Friends</span>
        <nav class="friends-panel__tabs">
          <button type="button" class="custom-tab" [class.active]="tabIndex === 0" (click)="tabIndex = 0">Pending</button>
          <button type="button" class="custom-tab" [class.active]="tabIndex === 1" (click)="tabIndex = 1">Add Friend</button>
        </nav>
      </header>

      <div class="friends-panel__body">
        @if (tabIndex === 1) {
          <div class="friends-panel__add">
            <h3>Add Friend</h3>
            <p>You can add friends with their KenVerse username.</p>
            <div class="friends-panel__input-row">
              <app-custom-input
                [(ngModel)]="addresseeName"
                placeholder="Enter a username"
                [dark]="true"
                (enterPressed)="sendRequest()"
              />
              <app-custom-button
                [disabled]="!addresseeName.trim()"
                (clicked)="sendRequest()"
              >
                Send Request
              </app-custom-button>
            </div>
            @if (messageSubmit) {
              <p class="friends-panel__feedback" [class.friends-panel__feedback--error]="isError">{{ messageSubmit }}</p>
            }
          </div>
        } @else {
          @if (friendPending.length === 0) {
            <div class="friends-panel__empty">
              <span nz-icon nzType="inbox" style="font-size: 40px; opacity: 0.3"></span>
              <p>No pending friend requests.</p>
            </div>
          } @else {
            @if (receivedRequests.length > 0) {
              <p class="friends-panel__group-title">Incoming — {{ receivedRequests.length }}</p>
              @for (friend of receivedRequests; track friend.id) {
                <div class="request-card">
                  <div class="kv-avatar request-card__avatar">
                    <img class="kv-avatar__img" [src]="friend.avatarUrl || '/logo.svg'" [alt]="friend.displayName" />
                  </div>
                  <div class="request-card__info">
                    <strong>{{ friend.displayName }}</strong>
                    <span>{{ friend.userName }}</span>
                  </div>
                  <div class="request-card__actions">
                    <app-custom-button [iconOnly]="true" (clicked)="accept(friend.id)">
                      <span nz-icon nzType="check"></span>
                    </app-custom-button>
                    <app-custom-button [iconOnly]="true" (clicked)="cancel(friend.id)">
                      <span nz-icon nzType="close"></span>
                    </app-custom-button>
                  </div>
                </div>
              }
            }
            @if (sentRequests.length > 0) {
              <p class="friends-panel__group-title">Outgoing — {{ sentRequests.length }}</p>
              @for (friend of sentRequests; track friend.id) {
                <div class="request-card">
                  <div class="kv-avatar request-card__avatar">
                    <img class="kv-avatar__img" [src]="friend.avatarUrl || '/logo.svg'" [alt]="friend.displayName" />
                  </div>
                  <div class="request-card__info">
                    <strong>{{ friend.displayName }}</strong>
                    <span>{{ friend.userName }}</span>
                  </div>
                  <app-custom-button [iconOnly]="true" (clicked)="cancel(friend.id)">
                    <span nz-icon nzType="close"></span>
                  </app-custom-button>
                </div>
              }
            }
          }
        }
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      height: 100%;
    }

    .friends-panel { background: #303031; display: flex; flex-direction: column; height: 100%; }

    .friends-panel__header {
      height: 59px;
      padding: 0 16px;
      display: flex;
      align-items: center;
      gap: 8px;
      border-bottom: 1px solid var(--kv-border-subtle);
      color: var(--kv-text-primary);
      font-weight: 600;
    }

    .friends-panel__tabs {
      display: flex;
      gap: 6px;
      margin-left: auto;
    }

    .friends-panel__tabs button {
      border: none;
      background: transparent;
    }

    .friends-panel__body {
      flex: 1;
      overflow-y: auto;
      padding: 20px;
    }

    .friends-panel__add h3 {
      margin: 0 0 8px;
      font-size: 20px;
    }

    .friends-panel__add p {
      margin: 0 0 20px;
      color: var(--kv-text-muted);
    }

    .friends-panel__input-row {
      display: grid;
      grid-template-columns: 1fr auto;
      gap: 10px;
      align-items: center;
    }

    .friends-panel__input-row app-custom-input {
      min-width: 0;
    }

    .friends-panel__feedback {
      margin-top: 12px;
      color: var(--kv-success);
      font-size: 13px;
    }

    .friends-panel__feedback--error { color: var(--kv-error); }

    .friends-panel__empty {
      height: 100%;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      gap: 12px;
      color: var(--kv-text-muted);
    }

    .friends-panel__group-title {
      margin: 16px 0 10px;
      font-size: 12px;
      font-weight: 700;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      color: var(--kv-text-muted);
    }

    .request-card {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 12px;
      margin-bottom: 8px;
      background: #2c2c2f;
      border-radius: var(--kv-radius-md);
      border: 1px solid var(--kv-border-subtle);
    }

    .request-card__avatar { width: 40px; height: 40px; flex-shrink: 0; }

    .request-card__info {
      flex: 1;
      min-width: 0;
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      gap: 2px;
    }

    .request-card__info strong {
      color: var(--kv-text-primary);
      font-size: 14px;
    }

    .request-card__info span {
      color: var(--kv-text-muted);
      font-size: 13px;
    }

    .request-card__actions {
      display: flex;
      gap: 8px;
    }
  `],
})
export class AddFriendSidebarComponent {
  @Input() friendPending: FriendPending[] = [];

  private readonly authState = inject(AuthStateService);
  private readonly api = inject(UserRelationshipApiService);
  private readonly state = inject(UserRelationshipStateService);

  tabIndex = 1;
  addresseeName = '';
  messageSubmit = '';
  isError = false;

  get receivedRequests(): FriendPending[] {
    return this.friendPending.filter((f) => f.isSender);
  }

  get sentRequests(): FriendPending[] {
    return this.friendPending.filter((f) => !f.isSender);
  }

  async sendRequest(): Promise<void> {
    const ownerId = this.authState.user?.id;
    if (!ownerId || !this.addresseeName.trim()) return;
    try {
      const response = await this.api.addFriend({ requesterId: ownerId, addresseeName: this.addresseeName });
      if (!response.isSuccess) throw new Error(response.message);
      this.state.addFriendPending({
        id: response.data.id,
        userName: response.data.userName,
        displayName: response.data.displayName,
        avatarUrl: response.data.avatarUrl,
        isSender: false,
      });
      this.messageSubmit = `Friend request sent to ${this.addresseeName}`;
      this.isError = false;
      this.addresseeName = '';
    } catch (err: unknown) {
      this.messageSubmit = err instanceof Error ? err.message : 'Add friend failed';
      this.isError = true;
    }
  }

  async accept(friendId: string): Promise<void> {
    const ownerId = this.authState.user?.id;
    if (!ownerId) return;
    const response = await this.api.updateUserRelationship({
      userID: ownerId,
      friendID: friendId,
      status: userRelationshipStatus.Accepted,
    });
    if (response.isSuccess) {
      this.state.removeFriendPending(friendId);
      this.state.addFriend(response.data);
    }
  }

  async cancel(friendId: string): Promise<void> {
    const ownerId = this.authState.user?.id;
    if (!ownerId) return;
    const response = await this.api.cancelFriendRequest({ userID: ownerId, friendID: friendId });
    if (response.isSuccess) this.state.removeFriendPending(friendId);
  }
}
