import { Component, EventEmitter, Input, Output, forwardRef } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-custom-input',
  standalone: true,
  imports: [FormsModule, NzInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomInputComponent),
      multi: true,
    },
  ],
  template: `
    <input
      nz-input
      [placeholder]="placeholder"
      [disabled]="disabled"
      [ngModel]="value"
      (ngModelChange)="onChange($event)"
      (blur)="onTouched()"
      [style]="customStyle"
      [class]="inputClass"
    />
  `,
})
export class CustomInputComponent implements ControlValueAccessor {
  @Input() placeholder = '';
  @Input() customStyle: Record<string, string> = {};
  @Input() inputClass = '';
  @Output() enterPressed = new EventEmitter<void>();

  value = '';
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
