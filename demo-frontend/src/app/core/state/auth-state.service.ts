import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthUser } from '../../shared/types/user';

@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly userSubject = new BehaviorSubject<AuthUser | null>(null);
  private readonly authenticatedSubject = new BehaviorSubject<boolean>(false);

  readonly user$ = this.userSubject.asObservable();
  readonly isAuthenticated$ = this.authenticatedSubject.asObservable();

  get user(): AuthUser | null {
    return this.userSubject.value;
  }

  get isAuthenticated(): boolean {
    return this.authenticatedSubject.value;
  }

  hydrateFromStorage(): void {
    const userStr = localStorage.getItem('user');
    const user = userStr && userStr !== 'undefined' ? JSON.parse(userStr) as AuthUser : null;
    if (user) {
      this.loginSuccess(user);
    }
  }

  loginSuccess(user: AuthUser): void {
    this.userSubject.next(user);
    this.authenticatedSubject.next(true);
  }

  logoutSuccess(): void {
    this.userSubject.next(null);
    this.authenticatedSubject.next(false);
  }
}
