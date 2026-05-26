import { Component, Input, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
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
  imports: [FormsModule, NzTabsModule, NzIconModule, NzToolTipModule, CustomInputComponent, CustomButtonComponent],
  template: `
    <div class="panel">
      <div class="header">
        <span nz-icon nzType="team"></span>
        <span style="margin-left: 8px">Friends</span>
        <nz-tabset [(nzSelectedIndex)]="tabIndex" nzSize="small" style="margin-left: 32px">
          <nz-tab nzTitle="Pending"></nz-tab>
          <nz-tab nzTitle="Add Friend"></nz-tab>
        </nz-tabset>
      </div>
      <div style="padding: 16px; color: #fff; flex: 1">
        @if (tabIndex === 1) {
          <h3>Add Friend</h3>
          <p>You can add friends with their KenVerse username</p>
          <div style="position: relative">
            <app-custom-input [(ngModel)]="addresseeName" [customStyle]="inputStyle" />
            <app-custom-button
              [buttonStyle]="{ position: 'absolute', top: '4px', right: '4px', backgroundColor: '#5865F2', color: '#fff' }"
              [disabled]="!addresseeName.trim()"
              (click)="sendRequest()"
            >
              Send Friend Request
            </app-custom-button>
          </div>
          @if (messageSubmit) { <p>{{ messageSubmit }}</p> }
        } @else {
          @if (friendPending.length === 0) {
            <p style="color: #aaa">No pending friend requests.</p>
          } @else {
            @for (friend of receivedRequests; track friend.id) {
              <div class="request-item">
                <div>
                  <strong>{{ friend.displayName }}</strong>
                  <div style="color: #aaa">{{ friend.userName }}</div>
                </div>
                <div>
                  <app-custom-button [buttonStyle]="btnStyle" (click)="accept(friend.id)"><span nz-icon nzType="check"></span></app-custom-button>
                  <app-custom-button [buttonStyle]="btnStyle" (click)="cancel(friend.id)"><span nz-icon nzType="stop"></span></app-custom-button>
                </div>
              </div>
            }
            @for (friend of sentRequests; track friend.id) {
              <div class="request-item">
                <div>
                  <strong>{{ friend.displayName }}</strong>
                  <div style="color: #aaa">{{ friend.userName }}</div>
                </div>
                <app-custom-button [buttonStyle]="btnStyle" (click)="cancel(friend.id)"><span nz-icon nzType="close"></span></app-custom-button>
              </div>
            }
          }
        }
      </div>
    </div>
  `,
  styles: [`
    .panel { background-color: rgb(48 48 49); display: flex; flex-direction: column; height: 100%; border-top-right-radius: 20px; border-bottom-right-radius: 20px; }
    .header { border-bottom: 1px solid #555; height: 59px; display: flex; align-items: center; padding-left: 16px; color: #fff; }
    .request-item { display: flex; justify-content: space-between; align-items: center; background-color: #2c2c2f; padding: 12px 16px; border-radius: 8px; margin-bottom: 12px; }
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
  inputStyle = { backgroundColor: '#212126', color: '#fff', borderColor: '#212126', paddingRight: '120px', height: '50px' };
  btnStyle = { width: '40px', height: '40px', color: '#fff', border: '1px solid #393b47', backgroundColor: '#393b47', borderRadius: '100%', marginRight: '12px' };

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
      this.messageSubmit = `Friend request sent successfully to ${this.addresseeName}`;
    } catch (err: unknown) {
      this.messageSubmit = err instanceof Error ? err.message : 'Add friend failed';
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
