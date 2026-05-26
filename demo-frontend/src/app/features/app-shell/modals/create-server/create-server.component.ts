import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { environment } from '../../../../../environments/environment';
import { ChannelApiService } from '../../../../core/services/channel-api.service';
import { CustomInputComponent } from '../../../../shared/components/custom-input/custom-input.component';

type Step = 'select' | 'create' | 'join';

@Component({
  selector: 'app-create-server',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NzModalModule,
    NzCardModule,
    NzButtonModule,
    NzFormModule,
    NzUploadModule,
    NzIconModule,
    CustomInputComponent,
  ],
  template: `
    <nz-modal
      [nzVisible]="open"
      [nzFooter]="null"
      [nzWidth]="400"
      nzCentered
      class="dark-modal"
      (nzOnCancel)="close()"
      [nzTitle]="title"
    >
      @if (step === 'select') {
        <div style="display: flex; flex-direction: column; gap: 16px">
          <nz-card nzHoverable (click)="step = 'create'" style="text-align: center; border-radius: 12px; background-color: #001529; border-color: #001529">
            <span nz-icon nzType="plus-circle" nzTheme="fill" style="font-size: 40px; color: #1677ff"></span>
            <h4 style="color: #fff">Create My Own</h4>
            <p style="color: #fff">Start a new server and invite friends</p>
          </nz-card>
          <nz-card nzHoverable (click)="step = 'join'" style="text-align: center; border-radius: 12px; background-color: #001529; border-color: #001529">
            <span nz-icon nzType="link" nzTheme="outline" style="font-size: 40px; color: #52c41a"></span>
            <h4 style="color: #fff">Join a Server</h4>
            <p style="color: #fff">Enter an invite link to join existing</p>
          </nz-card>
        </div>
      } @else {
        <form [formGroup]="form" (ngSubmit)="submit()" style="margin-top: 12px">
          @if (step === 'create') {
            <nz-upload
              nzName="IconUrl"
              nzListType="picture-card"
              class="server-uploader"
              [nzShowUploadList]="false"
              [nzAction]="uploadUrl"
              [nzHeaders]="uploadHeaders"
              (nzChange)="handleUpload($event)"
            >
              @if (imageUrl) {
                <img [src]="imageUrl" style="width: 100px; height: 100px; border-radius: 50%; object-fit: cover" />
              } @else {
                <div style="color: #fff">Upload</div>
              }
            </nz-upload>
            <nz-form-item nzLabel="Server Name">
              <app-custom-input formControlName="name" [customStyle]="inputStyle" inputClass="input-create-server" />
            </nz-form-item>
          } @else {
            <nz-form-item nzLabel="Invite Link">
              <app-custom-input formControlName="invite" placeholder="https://kendz.site/xyz" [customStyle]="inputStyle" />
            </nz-form-item>
            @if (errorMessage) {
              <p style="color: #dc3545">{{ errorMessage }}</p>
            }
          }
          <div style="display: flex; justify-content: space-between">
            <button nz-button nzType="link" type="button" style="color: #fff" (click)="back()">Back</button>
            <button nz-button nzType="primary" [nzLoading]="pending" [disabled]="isDisabled" type="submit" style="width: 100px; background-color: #5865f2">
              {{ step === 'create' ? 'Create' : 'Join' }}
            </button>
          </div>
        </form>
      }
    </nz-modal>
  `,
})
export class CreateServerComponent {
  @Input() open = false;
  @Input() ownerId?: string;
  @Output() closed = new EventEmitter<void>();

  private readonly fb = inject(FormBuilder);
  private readonly channelApi = inject(ChannelApiService);
  private readonly router = inject(Router);
  private readonly message = inject(NzMessageService);

  step: Step = 'select';
  pending = false;
  imageUrl = '';
  errorMessage = '';
  uploadUrl = `${environment.urlChannel}/server/UploadIcon`;
  uploadHeaders = { Authorization: `Bearer ${localStorage.getItem('token') || ''}` };
  inputStyle = { backgroundColor: '#212126', color: '#fff', borderColor: '#212126' };

  form = this.fb.group({ name: [''], invite: [''] });

  get title(): string {
    if (this.step === 'select') return 'Create Your Server';
    if (this.step === 'create') return 'Customize Your Server';
    return 'Join a Server';
  }

  get isDisabled(): boolean {
    return this.step === 'create' ? !this.form.value.name?.trim() : !this.form.value.invite?.trim();
  }

  handleUpload(info: { file: NzUploadFile }): void {
    if (info.file.status === 'done' && info.file.response?.data) {
      this.imageUrl = info.file.response.data;
    }
  }

  async submit(): Promise<void> {
    this.pending = true;
    this.errorMessage = '';
    try {
      if (this.step === 'create') {
        const server = await this.channelApi.createServerAndNavigate({
          name: this.form.value.name!,
          iconUrl: this.imageUrl || undefined,
          ownerId: this.ownerId,
        });
        this.message.success('Server created!');
        await this.router.navigate(['/server', server.id]);
      } else {
        const server = await this.channelApi.joinServerAndNavigate({
          Code: this.form.value.invite!,
          UserId: this.ownerId || '',
        });
        await this.router.navigate(['/server', server.id]);
      }
      this.close();
    } catch (err: unknown) {
      this.errorMessage = err instanceof Error ? err.message : 'Action failed';
    } finally {
      this.pending = false;
    }
  }

  back(): void {
    this.form.reset();
    this.errorMessage = '';
    this.step = 'select';
  }

  close(): void {
    this.form.reset();
    this.imageUrl = '';
    this.step = 'select';
    this.closed.emit();
  }
}
