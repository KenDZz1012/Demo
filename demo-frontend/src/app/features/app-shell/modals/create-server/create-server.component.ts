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
      [nzWidth]="440"
      nzCentered
      class="dark-modal create-server-modal"
      (nzOnCancel)="close()"
      [nzTitle]="title"
    >
      @if (step === 'select') {
        <p class="create-server-modal__desc">Your server is where you and your friends hang out.</p>
        <div class="create-server-modal__options">
          <button type="button" class="create-server-modal__option" (click)="step = 'create'">
            <span nz-icon nzType="plus-circle" nzTheme="fill" class="create-server-modal__icon create-server-modal__icon--blue"></span>
            <strong>Create My Own</strong>
            <span>Start fresh and invite friends</span>
          </button>
          <button type="button" class="create-server-modal__option" (click)="step = 'join'">
            <span nz-icon nzType="link" nzTheme="outline" class="create-server-modal__icon create-server-modal__icon--green"></span>
            <strong>Join a Server</strong>
            <span>Enter an invite link</span>
          </button>
        </div>
      } @else {
        <form nz-form nzLayout="vertical" [formGroup]="form" (ngSubmit)="submit()" class="create-server-modal__form">
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
                <img [src]="imageUrl" class="create-server-modal__preview" alt="Server icon" />
              } @else {
                <div class="create-server-modal__upload-placeholder">
                  <span nz-icon nzType="upload"></span>
                  <span>Upload</span>
                </div>
              }
            </nz-upload>
            <nz-form-item nzLabel="Server Name">
              <app-custom-input formControlName="name" placeholder="My awesome server" [dark]="true" />
            </nz-form-item>
          } @else {
            <nz-form-item nzLabel="Invite Link">
              <app-custom-input formControlName="invite" placeholder="Paste invite code or link" [dark]="true" />
            </nz-form-item>
            @if (errorMessage) {
              <p class="create-server-modal__error">{{ errorMessage }}</p>
            }
          }
          <div class="create-server-modal__actions">
            <button nz-button nzType="link" type="button" class="create-server-modal__back" (click)="back()">
              <span nz-icon nzType="arrow-left"></span> Back
            </button>
            <button nz-button nzType="primary" class="kv-btn-primary" [nzLoading]="pending" [disabled]="isDisabled" type="submit">
              {{ step === 'create' ? 'Create' : 'Join' }}
            </button>
          </div>
        </form>
      }
    </nz-modal>
  `,
  styles: [`
    .create-server-modal__desc {
      text-align: center;
      color: var(--kv-text-muted);
      margin: -8px 0 20px;
    }

    .create-server-modal__options {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }

    .create-server-modal__option {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 6px;
      padding: 20px;
      border: 1px solid var(--kv-border-subtle);
      border-radius: var(--kv-radius-lg);
      background: rgba(0, 0, 0, 0.2);
      color: var(--kv-text-primary);
      cursor: pointer;
      transition: border-color var(--kv-transition), background var(--kv-transition), transform 0.15s ease;
    }

    .create-server-modal__option:hover {
      border-color: var(--kv-blurple);
      background: rgba(88, 101, 242, 0.1);
      transform: translateY(-2px);
    }

    .create-server-modal__option strong { font-size: 16px; }
    .create-server-modal__option span:last-child { color: var(--kv-text-muted); font-size: 13px; }

    .create-server-modal__icon { font-size: 40px; }
    .create-server-modal__icon--blue { color: var(--kv-blurple-light); }
    .create-server-modal__icon--green { color: var(--kv-success); }

    .create-server-modal__form { margin-top: 8px; }

    .create-server-modal__preview {
      width: 100px;
      height: 100px;
      border-radius: 50%;
      object-fit: cover;
    }

    .create-server-modal__upload-placeholder {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      color: var(--kv-text-muted);
    }

    .create-server-modal__error {
      color: var(--kv-error);
      font-size: 13px;
      margin: 0 0 12px;
    }

    .create-server-modal__actions {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-top: 8px;
    }

    .create-server-modal__back {
      color: var(--kv-text-secondary) !important;
      padding-left: 0 !important;
    }
  `],
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
