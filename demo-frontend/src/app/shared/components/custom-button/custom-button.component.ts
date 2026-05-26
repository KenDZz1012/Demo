import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-custom-button',
  standalone: true,
  imports: [NzButtonModule],
  template: `
    <button
      nz-button
      [nzType]="type"
      [nzSize]="size"
      [disabled]="disabled"
      [nzLoading]="loading"
      [attr.type]="htmlType"
      [style]="buttonStyle"
      (mouseenter)="onEnter($event)"
      (mouseleave)="onLeave($event)"
      (click)="clicked.emit($event)"
    >
      <ng-content />
    </button>
  `,
})
export class CustomButtonComponent {
  @Input() type: 'primary' | 'default' | 'link' = 'default';
  @Input() size: 'large' | 'default' | 'small' = 'default';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() htmlType: 'button' | 'submit' = 'button';
  @Input() bgColor = '#5865F2';
  @Input() hoverColor = '#4752C4';
  @Input() buttonStyle: Record<string, string> = {};
  @Output() clicked = new EventEmitter<MouseEvent>();

  onEnter(event: MouseEvent): void {
    (event.currentTarget as HTMLElement).style.backgroundColor = this.hoverColor;
  }

  onLeave(event: MouseEvent): void {
    (event.currentTarget as HTMLElement).style.backgroundColor = this.bgColor;
  }
}
