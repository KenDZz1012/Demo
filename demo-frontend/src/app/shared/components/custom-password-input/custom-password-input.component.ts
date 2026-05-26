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
    <nz-input-group [nzSuffix]="suffixTemplate">
      <input
        nz-input
        [type]="visible ? 'text' : 'password'"
        [placeholder]="placeholder"
        [disabled]="disabled"
        [(ngModel)]="value"
        (ngModelChange)="onChange($event)"
        (blur)="onTouched()"
        [style]="customStyle"
      />
    </nz-input-group>
    <ng-template #suffixTemplate>
      @if (visibilityToggle) {
        <span style="cursor: pointer" (click)="visible = !visible">{{ visible ? '🙈' : '👁' }}</span>
      }
    </ng-template>
  `,
})
export class CustomPasswordInputComponent implements ControlValueAccessor {
  @Input() placeholder = '';
  @Input() customStyle: Record<string, string> = {};
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
