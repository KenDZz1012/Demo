import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-custom-button',
  standalone: true,
  imports: [NzButtonModule],
  template: `
    <button
      nz-button
      class="kv-btn-primary"
      [class.kv-btn-block]="block"
      [class.kv-btn-icon]="iconOnly"
      [nzType]="type"
      [nzSize]="size"
      [disabled]="disabled"
      [nzLoading]="loading"
      [attr.type]="htmlType"
      [style]="buttonStyle"
      (click)="clicked.emit($event)"
    >
      <ng-content />
    </button>
  `,
  styles: [`
    :host {
      display: block;
      width: 100%;
    }

    .kv-btn-block {
      width: 100%;
    }
    .kv-btn-icon {
      width: 40px !important;
      height: 40px !important;
      min-width: 40px !important;
      padding: 0 !important;
      border-radius: 50% !important;
    }
  `],
})
export class CustomButtonComponent {
  @Input() type: 'primary' | 'default' | 'link' = 'primary';
  @Input() size: 'large' | 'default' | 'small' = 'default';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() block = false;
  @Input() iconOnly = false;
  @Input() htmlType: 'button' | 'submit' = 'button';
  @Input() buttonStyle: Record<string, string> = {};
  @Output() clicked = new EventEmitter<MouseEvent>();
}
