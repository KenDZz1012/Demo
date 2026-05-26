import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-custom-select',
  standalone: true,
  imports: [FormsModule, NzSelectModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomSelectComponent),
      multi: true,
    },
  ],
  template: `
    <nz-select
      [nzPlaceHolder]="placeholder"
      [nzOptions]="options"
      [nzShowSearch]="showSearch"
      [nzAllowClear]="allowClear"
      [nzDisabled]="disabled"
      [(ngModel)]="value"
      (ngModelChange)="onChange($event)"
      (blur)="onTouched()"
      [style]="customStyle"
      [class]="selectClass"
    />
  `,
})
export class CustomSelectComponent implements ControlValueAccessor {
  @Input() placeholder = '';
  @Input() options: Array<{ label: string | number; value: string | number }> = [];
  @Input() customStyle: Record<string, string> = {};
  @Input() selectClass = '';
  @Input() showSearch = false;
  @Input() allowClear = false;

  value: string | number | null = null;
  disabled = false;
  onChangeFn: (value: string | number | null) => void = () => {};
  onTouched: () => void = () => {};

  writeValue(value: string | number | null): void {
    this.value = value;
  }

  registerOnChange(fn: (value: string | number | null) => void): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onChange(value: string | number | null): void {
    this.value = value;
    this.onChangeFn(value);
  }
}
