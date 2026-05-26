import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-server-chat-area',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, NzIconModule],
  template: `
    <div class="kv-chat-area">
      <header class="kv-chat-area__header">
        <span class="server-chat__hash">#</span>
        <h2 class="kv-chat-area__title">{{ channelName || 'general' }}</h2>
      </header>

      <div class="kv-chat-area__messages">
        @if (messages.length === 0) {
          <div class="kv-chat-area__empty">
            <span nz-icon nzType="comment" style="font-size: 48px; opacity: 0.3"></span>
            <p>Welcome to <strong>#{{ channelName }}</strong>!</p>
            <span>This is the start of the channel.</span>
          </div>
        } @else {
          @for (msg of messages; track $index) {
            <div class="kv-message">{{ msg }}</div>
          }
        }
      </div>

      <div class="kv-chat-area__composer kv-composer">
        <nz-input-group nzSearch [nzAddOnAfter]="suffixButton">
          <input
            nz-input
            [placeholder]="'Message #' + (channelName || 'channel')"
            [(ngModel)]="inputValue"
            (ngModelChange)="inputValueChange.emit($event)"
            (keyup.enter)="send.emit()"
          />
        </nz-input-group>
        <ng-template #suffixButton>
          <button nz-button nzType="primary" nzSearch (click)="send.emit()">Send</button>
        </ng-template>
      </div>
    </div>
  `,
  styles: [`
    .server-chat__hash {
      color: var(--kv-text-muted);
      font-size: 22px;
      font-weight: 700;
    }
  `],
})
export class ServerChatAreaComponent {
  @Input() channelName?: string;
  @Input() messages: string[] = [];
  @Input() inputValue = '';
  @Output() inputValueChange = new EventEmitter<string>();
  @Output() send = new EventEmitter<void>();
}
