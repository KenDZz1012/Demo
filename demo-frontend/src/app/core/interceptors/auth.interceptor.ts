import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, from, switchMap, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../services/auth.service';
import { SignalRService } from '../services/signalr.service';

let isRefreshing = false;
let failedQueue: Array<{ resolve: (token: string | null) => void; reject: (err: unknown) => void }> = [];

const processQueue = (error: unknown, token: string | null = null) => {
  failedQueue.forEach((prom) => {
    if (error) prom.reject(error);
    else prom.resolve(token);
  });
  failedQueue = [];
};

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const signalR = inject(SignalRService);

  const token = localStorage.getItem('token');
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const originalRequest = authReq;

      if (
        error.status === 401 &&
        !originalRequest.url.includes('/login') &&
        !originalRequest.url.includes('/refresh')
      ) {
        if (isRefreshing) {
          return from(
            new Promise<string | null>((resolve, reject) => {
              failedQueue.push({ resolve, reject });
            })
          ).pipe(
            switchMap((newToken) => {
              if (!newToken) return throwError(() => error);
              return next(
                originalRequest.clone({ setHeaders: { Authorization: `Bearer ${newToken}` } })
              );
            })
          );
        }

        isRefreshing = true;
        const refreshToken = localStorage.getItem('refreshToken');
        const authUser = JSON.parse(localStorage.getItem('user') || 'null') as { id?: string } | null;

        return from(
          fetch(`${environment.urlAuth}/refresh`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ refreshToken, userId: authUser?.id }),
          }).then(async (res) => {
            if (!res.ok) throw new Error('Refresh failed');
            return res.json();
          })
        ).pipe(
          switchMap((body: { data: { accessToken: string; refreshToken: string } }) => {
            const { accessToken, refreshToken: newRefreshToken } = body.data;
            if (accessToken) localStorage.setItem('token', accessToken);
            if (newRefreshToken) localStorage.setItem('refreshToken', newRefreshToken);
            return from(signalR.restartConnection()).pipe(
              switchMap(() => {
                processQueue(null, accessToken);
                isRefreshing = false;
                return next(
                  originalRequest.clone({ setHeaders: { Authorization: `Bearer ${accessToken}` } })
                );
              })
            );
          }),
          catchError((err) => {
            processQueue(err, null);
            isRefreshing = false;
            authService.handleForcedLogout();
            return throwError(() => err);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
