import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { CreateChannel } from '../../../../shared/types/channel';
import { CustomInputComponent } from '../../../../shared/components/custom-input/custom-input.component';
import { CustomButtonComponent } from '../../../../shared/components/custom-button/custom-button.component';

@Component({
  selector: 'app-create-channel',
  standalone: true,
  imports: [ReactiveFormsModule, NzModalModule, NzRadioModule, NzFormModule, NzIconModule, CustomInputComponent, CustomButtonComponent],
  template: `
    <nz-modal [nzVisible]="visible" nzTitle="Create Channel" class="dark-modal" [nzFooter]="null" (nzOnCancel)="cancel()">
      <form nz-form nzLayout="vertical" [formGroup]="form" (ngSubmit)="submit()">
        <nz-radio-group formControlName="type" style="width: 100%">
          <label nz-radio nzValue="text" style="color: #fff">Text</label>
          <label nz-radio nzValue="voice" style="color: #fff">Voice</label>
        </nz-radio-group>
        <nz-form-item nzLabel="Channel Name" style="margin-top: 16px">
          <app-custom-input formControlName="name" placeholder="new-channel" [customStyle]="inputStyle" />
        </nz-form-item>
        <div style="display: flex; justify-content: flex-end; gap: 10px">
          <app-custom-button [buttonStyle]="cancelStyle" (click)="cancel()">Cancel</app-custom-button>
          <app-custom-button type="primary" htmlType="submit" [buttonStyle]="{ width: '140px', backgroundColor: '#5865f2' }">Create channel</app-custom-button>
        </div>
      </form>
    </nz-modal>
  `,
})
export class CreateChannelComponent {
  @Input() visible = false;
  @Output() cancelled = new EventEmitter<void>();
  @Output() created = new EventEmitter<CreateChannel>();

  private readonly fb = inject(FormBuilder);
  inputStyle = { backgroundColor: '#212126', color: '#fff', borderColor: '#212126' };
  cancelStyle = { color: '#fff', border: '1px solid #393b47', backgroundColor: '#393b47', width: '100px' };

  form = this.fb.group({ name: [''], type: ['text'] });

  submit(): void {
    const value = this.form.getRawValue();
    if (!value.name?.trim()) return;
    this.created.emit({ name: value.name, type: value.type || 'text', serverId: '' });
    this.form.reset({ type: 'text' });
  }

  cancel(): void {
    this.form.reset({ type: 'text' });
    this.cancelled.emit();
  }
}
