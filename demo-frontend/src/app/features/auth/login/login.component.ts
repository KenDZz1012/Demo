import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { AuthService } from '../../../core/services/auth.service';
import { AuthStateService } from '../../../core/state/auth-state.service';
import { CustomButtonComponent } from '../../../shared/components/custom-button/custom-button.component';
import { CustomInputComponent } from '../../../shared/components/custom-input/custom-input.component';
import { CustomPasswordInputComponent } from '../../../shared/components/custom-password-input/custom-password-input.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NzFormModule,
    NzTypographyModule,
    CustomButtonComponent,
    CustomInputComponent,
    CustomPasswordInputComponent,
  ],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly authState = inject(AuthStateService);
  private readonly router = inject(Router);

  loading = false;
  errors: Record<string, string> = {};

  form = this.fb.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor() {
    if (this.authState.isAuthenticated) {
      this.router.navigate(['/server/@me'], { replaceUrl: true });
    }
  }

  async submit(): Promise<void> {
    this.errors = {};
    if (this.form.invalid) {
      this.errors = { userName: 'Required', password: 'Required' };
      return;
    }

    this.loading = true;
    try {
      await this.authService.login(this.form.getRawValue() as { userName: string; password: string });
      await this.router.navigate(['/server/@me'], { replaceUrl: true });
    } catch {
      this.errors = {
        userName: 'Invalid email or password',
        password: 'Invalid email or password',
      };
    } finally {
      this.loading = false;
    }
  }
}
