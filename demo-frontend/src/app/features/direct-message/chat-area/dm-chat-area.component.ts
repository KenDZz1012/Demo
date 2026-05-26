import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { Friend } from '../../../shared/types/user';

@Component({
  selector: 'app-dm-chat-area',
  standalone: true,
  imports: [FormsModule, NzInputModule],
  template: `
    <div class="chat-area">
      <div class="header">
        <div style="position: relative; width: 30px; height: 30px">
          <img [src]="friend?.avatarUrl || '/logo.svg'" [alt]="friend?.displayName" style="width: 100%; height: 100%; border-radius: 50%; object-fit: cover" />
          <span [style.backgroundColor]="friend?.isOnline ? 'green' : 'gray'" style="position: absolute; bottom: 0; right: 0; width: 10px; height: 10px; border-radius: 50%; border: 2px solid white"></span>
        </div>
        <h3 style="color: white; margin-left: 10px">{{ friend?.displayName }}</h3>
      </div>
      <div class="messages"></div>
      <div style="padding: 20px">
        <nz-input-group nzSearch [nzAddOnAfter]="suffixButton">
          <input nz-input placeholder="Type your message..." [(ngModel)]="inputValue" (keyup.enter)="send.emit()" />
        </nz-input-group>
        <ng-template #suffixButton>
          <button nz-button nzType="primary" nzSearch (click)="send.emit()">Send</button>
        </ng-template>
      </div>
    </div>
  `,
  styles: [`
    .chat-area { background-color: #31323d; display: flex; flex-direction: column; height: 100%; border-top-right-radius: 20px; border-bottom-right-radius: 20px; }
    .header { border-bottom: 1px solid #555; height: 59px; display: flex; align-items: center; padding-left: 16px; }
    .messages { flex: 1; overflow-y: auto; padding: 20px; }
  `],
})
export class DmChatAreaComponent {
  @Input() friend: Friend | null | undefined;
  @Input() inputValue = '';
  @Output() inputValueChange = new EventEmitter<string>();
  @Output() send = new EventEmitter<void>();
}
