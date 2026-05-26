import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-custom-password-input',
  standalone: true,
  imports: [FormsModule, NzInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomPasswordInputComponent),
      multi: true,
    },
  ],
  template: `
    @if (visibilityToggle) {
      <nz-input-group [nzSuffix]="suffixTemplate" class="kv-input-group">
        <input
          nz-input
          class="kv-input"
          [type]="visible ? 'text' : 'password'"
          [placeholder]="placeholder"
          [disabled]="disabled"
          [(ngModel)]="value"
          (ngModelChange)="onChange($event)"
          (blur)="onTouched()"
        />
      </nz-input-group>
      <ng-template #suffixTemplate>
        <button type="button" class="kv-toggle-visibility" (click)="visible = !visible">
          {{ visible ? 'Hide' : 'Show' }}
        </button>
      </ng-template>
    } @else {
      <input
        nz-input
        class="kv-input"
        type="password"
        [placeholder]="placeholder"
        [disabled]="disabled"
        [(ngModel)]="value"
        (ngModelChange)="onChange($event)"
        (blur)="onTouched()"
      />
    }
  `,
  styles: [`
    :host {
      display: block;
      width: 100%;
    }

    .kv-toggle-visibility {
      background: none;
      border: none;
      color: var(--kv-text-muted);
      font-size: 12px;
      font-weight: 600;
      cursor: pointer;
      padding: 0 4px;
    }

    .kv-toggle-visibility:hover {
      color: var(--kv-text-secondary);
    }
  `],
})
export class CustomPasswordInputComponent implements ControlValueAccessor {
  @Input() placeholder = '';
  @Input() visibilityToggle = true;

  value = '';
  visible = false;
  disabled = false;
  onChangeFn: (value: string) => void = () => {};
  onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value = value ?? '';
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onChange(value: string): void {
    this.value = value;
    this.onChangeFn(value);
  }
}
