import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-server-chat-area',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule],
  template: `
    <div class="chat-area">
      <div class="header"><h2 style="color: white">#{{ channelName }}</h2></div>
      <div class="messages">
        @for (msg of messages; track $index) {
          <div class="message">{{ msg }}</div>
        }
      </div>
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
    .header { border-bottom: 1px solid #555; height: 51px; display: flex; align-items: center; padding-left: 16px; }
    .messages { flex: 1; overflow-y: auto; padding: 20px; }
    .message { background: #4f545c; padding: 8px; margin-bottom: 4px; border-radius: 4px; color: white; }
  `],
})
export class ServerChatAreaComponent {
  @Input() channelName?: string;
  @Input() messages: string[] = [];
  @Input() inputValue = '';
  @Output() inputValueChange = new EventEmitter<string>();
  @Output() send = new EventEmitter<void>();
}
