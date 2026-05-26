import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { CustomInputComponent } from '../../../shared/components/custom-input/custom-input.component';
import { Friend } from '../../../shared/types/user';

@Component({
  selector: 'app-list-friend-sidebar',
  standalone: true,
  imports: [NzMenuModule, NzIconModule, CustomInputComponent],
  template: `
    <ul nz-menu nzTheme="dark" nzMode="inline" class="channel-menu sidebar">
      <div style="padding: 0 10px 10px; border-bottom: 1px solid #555">
        <app-custom-input placeholder="Find friends" [customStyle]="{ backgroundColor: '#212126', color: '#fff', borderColor: '#212126' }" />
      </div>
      <li nz-menu-item (click)="selectedFriend.emit(null)">
        <span nz-icon nzType="team"></span>
        <span style="margin-left: 10px">Friends</span>
      </li>
      <li nz-menu-group nzTitle="Direct Messages">
        @for (friend of friends; track friend.id) {
          <li nz-menu-item [nzSelected]="friendId === friend.id" (click)="selectedFriend.emit(friend)">
            <div style="display: flex; align-items: center; gap: 10px">
              <div style="position: relative; width: 36px; height: 36px">
                <img [src]="friend.avatarUrl || '/logo.svg'" [alt]="friend.displayName" style="width: 100%; height: 100%; border-radius: 50%; object-fit: cover" />
                <span [style.backgroundColor]="friend.isOnline ? 'green' : 'gray'" style="position: absolute; bottom: 0; right: 0; width: 10px; height: 10px; border-radius: 50%; border: 2px solid white"></span>
              </div>
              <span>{{ friend.displayName }}</span>
            </div>
          </li>
        }
      </li>
    </ul>
  `,
  styles: [`
    .sidebar {
      background-color: #2a2c35;
      color: white;
      border-top-left-radius: 20px;
      border-bottom-left-radius: 20px;
      padding-top: 8px;
      border-right: 1px solid #555;
    }
  `],
})
export class ListFriendSidebarComponent {
  @Input() friends: Friend[] = [];
  @Input() friendId?: string | null;
  @Output() selectedFriend = new EventEmitter<Friend | null>();
}
