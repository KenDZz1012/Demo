import { Injectable, inject, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SignalRService } from '../services/signalr.service';
import { UserRelationshipStateService } from '../state/user-relationship-state.service';

@Injectable({ providedIn: 'root' })
export class SignalRListenerService implements OnDestroy {
  private readonly signalR = inject(SignalRService);
  private readonly userRelationshipState = inject(UserRelationshipStateService);
  private readonly subscriptions = new Subscription();

  init(): void {
    this.subscriptions.add(
      this.signalR.getEvent$<{
        fromUserId: string;
        fromUserName: string;
        fromUserDisplayName: string;
        fromUserAvatarUrl?: string;
      }>('friendRequestReceived').subscribe((payload) => {
        this.userRelationshipState.addFriendPending({
          id: payload.fromUserId,
          userName: payload.fromUserName,
          displayName: payload.fromUserDisplayName,
          isSender: true,
          avatarUrl: payload.fromUserAvatarUrl || '',
        });
      })
    );

    this.subscriptions.add(
      this.signalR.getEvent$<{
        fromUserId: string;
        fromUserName: string;
        fromUserDisplayName: string;
        fromUserAvatarUrl?: string;
      }>('friendRequestAccepted').subscribe((payload) => {
        this.userRelationshipState.addFriend({
          id: payload.fromUserId,
          userName: payload.fromUserName,
          displayName: payload.fromUserDisplayName,
          avatarUrl: payload.fromUserAvatarUrl || '',
          isOnline: true,
        });
        this.userRelationshipState.removeFriendPending(payload.fromUserId);
      })
    );

    this.subscriptions.add(
      this.signalR.getEvent$<{ fromUserId: string }>('friendRequestRejected').subscribe((payload) => {
        this.userRelationshipState.removeFriendPending(payload.fromUserId);
      })
    );

    this.subscriptions.add(
      this.signalR.getEvent$<{ userName: string; isOnline: boolean }>('friendStatusChanged').subscribe((payload) => {
        this.userRelationshipState.setStatusFriend(payload);
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
