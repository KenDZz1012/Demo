import { Component, OnInit, inject } from '@angular/core';
import { AuthStateService } from '../../core/state/auth-state.service';
import { UserRelationshipApiService } from '../../core/services/user-relationship-api.service';
import { UserRelationshipStateService } from '../../core/state/user-relationship-state.service';
import { DirectMessageApiService } from '../../core/services/direct-message-api.service';
import { ListFriendSidebarComponent } from './list-friend-sidebar/list-friend-sidebar.component';
import { AddFriendSidebarComponent } from './add-friend-sidebar/add-friend-sidebar.component';
import { DmChatAreaComponent } from './chat-area/dm-chat-area.component';
import { Friend } from '../../shared/types/user';

@Component({
  selector: 'app-direct-message',
  standalone: true,
  imports: [ListFriendSidebarComponent, AddFriendSidebarComponent, DmChatAreaComponent],
  template: `
    <div class="dm-layout">
      <app-list-friend-sidebar
        class="dm-layout__sidebar"
        [friends]="userRelationshipState.friends"
        [friendId]="userRelationshipState.selectedFriendId"
        (selectedFriend)="onSelectedFriend($event)"
      />

      <div class="dm-layout__content">
        @if (userRelationshipState.selectedFriendId) {
          <app-dm-chat-area
            [friend]="userRelationshipState.selectedFriend"
            [(inputValue)]="input"
            (send)="sendMessage()"
          />
        } @else {
          <app-add-friend-sidebar [friendPending]="userRelationshipState.friendsPending" />
        }
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      height: 100%;
    }

    .dm-layout {
      display: grid;
      grid-template-columns: 300px 1fr;
      height: 100%;
      gap: 0;
    }

    .dm-layout__sidebar {
      min-height: 0;
      height: 100%;
      overflow: hidden;
    }

    .dm-layout__content {
      min-height: 0;
      height: 100%;
      overflow: hidden;
    }
  `],
})
export class DirectMessageComponent implements OnInit {
  readonly authState = inject(AuthStateService);
  readonly userRelationshipState = inject(UserRelationshipStateService);
  private readonly userRelationshipApi = inject(UserRelationshipApiService);
  private readonly directMessageApi = inject(DirectMessageApiService);

  input = '';

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    const userID = this.authState.user?.id;
    if (!userID) return;
    const [friends, pending] = await Promise.all([
      this.userRelationshipApi.fetchFriends({ userID }),
      this.userRelationshipApi.fetchFriendsPending({ userID }),
    ]);
    if (friends.isSuccess) this.userRelationshipState.setFriends(friends.data);
    if (pending.isSuccess) this.userRelationshipState.setFriendsPending(pending.data);
  }

  onSelectedFriend(friend: Friend | null): void {
    this.userRelationshipState.setSelectedFriend(friend);
  }

  async sendMessage(): Promise<void> {
    const friendId = this.userRelationshipState.selectedFriendId;
    const userID = this.authState.user?.id;
    if (!this.input.trim() || !friendId || !userID) return;
    await this.directMessageApi.sendMessage({
      senderId: userID,
      recipientIds: [friendId],
      content: this.input.trim(),
    });
    this.input = '';
  }
}
