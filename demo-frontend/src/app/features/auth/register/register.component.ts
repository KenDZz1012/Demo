import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzMessageService } from 'ng-zorro-antd/message';
import { AuthService } from '../../../core/services/auth.service';
import { CustomButtonComponent } from '../../../shared/components/custom-button/custom-button.component';
import { CustomInputComponent } from '../../../shared/components/custom-input/custom-input.component';
import { CustomPasswordInputComponent } from '../../../shared/components/custom-password-input/custom-password-input.component';
import { CustomSelectComponent } from '../../../shared/components/custom-select/custom-select.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NzFormModule,
    CustomButtonComponent,
    CustomInputComponent,
    CustomPasswordInputComponent,
    CustomSelectComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly message = inject(NzMessageService);

  loading = false;
  invalidDate = false;
  errors: Record<string, string> = {};

  years = Array.from({ length: new Date().getFullYear() - 1979 }, (_, i) => ({
    value: 1980 + i,
    label: 1980 + i,
  }));
  months = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ].map((month, index) => ({ value: index + 1, label: month }));
  days = Array.from({ length: 31 }, (_, i) => ({ value: i + 1, label: i + 1 }));

  inputStyle = { backgroundColor: '#28282d', color: '#d0d1d3', borderColor: '#40444b' };
  selectStyle = { width: '30%', borderRadius: '8px' };

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    displayName: ['', Validators.required],
    userName: ['', Validators.required],
    passwordHash: ['', Validators.required],
    day: [null as number | null, Validators.required],
    month: [null as number | null, Validators.required],
    year: [null as number | null, Validators.required],
  });

  validateDate(): void {
    const day = this.form.value.day;
    const month = this.form.value.month;
    const year = this.form.value.year;
    if (!day || !month || !year) {
      this.invalidDate = false;
      return;
    }
    const date = new Date(year, month - 1, day);
    this.invalidDate = !(
      date.getFullYear() === year &&
      date.getMonth() === month - 1 &&
      date.getDate() === day
    );
    if (this.invalidDate) {
      this.errors['dateOfBirth'] = 'Invalid date';
    } else {
      delete this.errors['dateOfBirth'];
    }
  }

  async submit(): Promise<void> {
    this.validateDate();
    if (this.form.invalid || this.invalidDate) {
      this.errors = { ...this.errors, form: 'Invalid' };
      return;
    }

    const { email, displayName, userName, passwordHash, day, month, year } = this.form.getRawValue();
    const dateOfBirth = day && month && year
      ? new Date(Date.UTC(year, month - 1, day)).toISOString().slice(0, 10)
      : undefined;

    this.loading = true;
    try {
      await this.authService.register({
        email: email!,
        displayName: displayName!,
        userName: userName!,
        passwordHash: passwordHash!,
        dateOfBirth,
      });
      this.message.success('Account created successfully!');
      await this.authService.login({ userName: userName!, password: passwordHash! });
      await this.router.navigate(['/server/@me'], { replaceUrl: true });
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Registration failed';
      this.message.error(message);
    } finally {
      this.loading = false;
    }
  }
}
