import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Friend } from '../../../shared/types/user';

@Component({
  selector: 'app-dm-chat-area',
  standalone: true,
  imports: [FormsModule, NzInputModule, NzButtonModule, NzIconModule],
  template: `
    <div class="kv-chat-area">
      <header class="kv-chat-area__header">
        <div class="kv-avatar" style="width: 32px; height: 32px">
          <img class="kv-avatar__img" [src]="friend?.avatarUrl || '/logo.svg'" [alt]="friend?.displayName" />
          <span
            class="kv-avatar__status"
            [class.kv-avatar__status--online]="friend?.isOnline"
            [class.kv-avatar__status--offline]="!friend?.isOnline"
          ></span>
        </div>
        <h3 class="kv-chat-area__title">{{ friend?.displayName }}</h3>
      </header>

      <div class="kv-chat-area__messages">
        <div class="kv-chat-area__empty">
          <span nz-icon nzType="message" style="font-size: 48px; opacity: 0.3"></span>
          <p>This is the beginning of your direct message history with <strong>{{ friend?.displayName }}</strong>.</p>
        </div>
      </div>

      <div class="kv-chat-area__composer kv-composer">
        <nz-input-group nzSearch [nzAddOnAfter]="suffixButton">
          <input
            nz-input
            placeholder="Message @{{ friend?.userName }}"
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
    :host {
      display: block;
      height: 100%;
    }
  `],
})
export class DmChatAreaComponent {
  @Input() friend: Friend | null | undefined;
  @Input() inputValue = '';
  @Output() inputValueChange = new EventEmitter<string>();
  @Output() send = new EventEmitter<void>();
}
