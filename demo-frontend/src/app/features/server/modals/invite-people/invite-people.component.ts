import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { ServerDetail } from '../../../../shared/types/server';
import { CustomInputComponent } from '../../../../shared/components/custom-input/custom-input.component';
import { CustomButtonComponent } from '../../../../shared/components/custom-button/custom-button.component';

@Component({
  selector: 'app-invite-people',
  standalone: true,
  imports: [NzModalModule, NzIconModule, CustomInputComponent, CustomButtonComponent],
  template: `
    <nz-modal
      [nzVisible]="visible"
      class="dark-modal"
      [nzFooter]="null"
      (nzOnCancel)="cancelled.emit()"
      [nzTitle]="'Invite people to ' + (server?.name || '')"
    >
      <app-custom-input placeholder="Search for friends" [customStyle]="inputStyle" />
      <p style="color: #fff; margin-top: 16px">Send a server invite link to a friend</p>
      <div class="code-box">
        {{ server?.code }}
        <app-custom-button [buttonStyle]="{ border: 'none', backgroundColor: '#5865f2' }" (click)="copy()">
          <span nz-icon nzType="copy"></span>
        </app-custom-button>
      </div>
    </nz-modal>
  `,
  styles: [`
    .code-box {
      background-color: #2f3136;
      padding: 10px;
      border-radius: 5px;
      margin-top: 8px;
      color: #ebebeb;
      font-size: 16px;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }
  `],
})
export class InvitePeopleComponent {
  @Input() visible = false;
  @Input() server: ServerDetail | null = null;
  @Output() cancelled = new EventEmitter<void>();

  inputStyle = { backgroundColor: '#212126', color: '#fff', borderColor: '#212126' };

  copy(): void {
    if (this.server?.code) {
      navigator.clipboard.writeText(this.server.code);
    }
  }
}
