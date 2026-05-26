import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Friend, FriendPending } from '../../shared/types/user';

@Injectable({ providedIn: 'root' })
export class UserRelationshipStateService {
  private readonly friendsSubject = new BehaviorSubject<Friend[]>([]);
  private readonly friendsPendingSubject = new BehaviorSubject<FriendPending[]>([]);
  private readonly selectedFriendSubject = new BehaviorSubject<Friend | null>(null);

  readonly friends$ = this.friendsSubject.asObservable();
  readonly friendsPending$ = this.friendsPendingSubject.asObservable();
  readonly selectedFriend$ = this.selectedFriendSubject.asObservable();

  get friends(): Friend[] {
    return this.friendsSubject.value;
  }

  get friendsPending(): FriendPending[] {
    return this.friendsPendingSubject.value;
  }

  get selectedFriend(): Friend | null {
    return this.selectedFriendSubject.value;
  }

  get selectedFriendId(): string | null | undefined {
    return this.selectedFriendSubject.value?.id;
  }

  setFriends(friends: Friend[]): void {
    this.friendsSubject.next(friends);
  }

  setFriendsPending(friends: FriendPending[]): void {
    this.friendsPendingSubject.next(friends);
  }

  addFriend(friend: Friend): void {
    const exists = this.friends.some((f) => f.id === friend.id);
    if (!exists) {
      this.friendsSubject.next([...this.friends, friend]);
    }
  }

  addFriendPending(friend: FriendPending): void {
    const exists = this.friendsPending.some((f) => f.id === friend.id);
    if (!exists) {
      this.friendsPendingSubject.next([...this.friendsPending, friend]);
    }
  }

  removeFriendPending(friendId: string): void {
    this.friendsPendingSubject.next(this.friendsPending.filter((f) => f.id !== friendId));
  }

  setStatusFriend(payload: { userName: string; isOnline: boolean }): void {
    this.friendsSubject.next(
      this.friends.map((friend) =>
        friend.userName === payload.userName ? { ...friend, isOnline: payload.isOnline } : friend
      )
    );
  }

  setSelectedFriend(friend: Friend | null): void {
    this.selectedFriendSubject.next(friend);
  }
}
