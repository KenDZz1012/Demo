import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { en_US, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { AuthStateService } from './core/state/auth-state.service';
import { SignalRService } from './core/services/signalr.service';
import { SignalRListenerService } from './core/services/signalr-listener.service';
import { provideAppIcons } from './icons.provider';

registerLocaleData(en);

function initializeApp(
  authState: AuthStateService,
  signalR: SignalRService,
  signalRListener: SignalRListenerService
) {
  return () => {
    authState.hydrateFromStorage();
    signalRListener.init();
    if (localStorage.getItem('token')) {
      return signalR.startConnection();
    }
    return Promise.resolve();
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimations(),
    provideNzI18n(en_US),
    provideAppIcons(),
    importProvidersFrom(FormsModule, ReactiveFormsModule),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AuthStateService, SignalRService, SignalRListenerService],
      multi: true,
    },
  ],
};
