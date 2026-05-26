import { Injectable, inject } from '@angular/core';
import { Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private heartbeatInterval: ReturnType<typeof setInterval> | null = null;
  private readonly registeredEvents = new Set<string>();
  private readonly eventSubjects = new Map<string, Subject<unknown>>();

  getEvent$<T = unknown>(eventName: string) {
    if (!this.eventSubjects.has(eventName)) {
      this.eventSubjects.set(eventName, new Subject<unknown>());
    }
    return this.eventSubjects.get(eventName)!.asObservable() as import('rxjs').Observable<T>;
  }

  async startConnection(): Promise<void> {
    if (this.connection) return;

    const token = localStorage.getItem('token');
    if (!token) {
      console.error('Token is missing. Cannot start SignalR connection.');
      return;
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.urlPresence, {
        accessTokenFactory: () => localStorage.getItem('token') || '',
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    try {
      await this.connection.start();
      this.startHeartbeat(this.connection);
      this.registerEvent('friendRequestReceived');
      this.registerEvent('friendRequestAccepted');
      this.registerEvent('friendRequestRejected');
      this.registerEvent('friendStatusChanged');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : String(err);
      console.error('SignalR connection failed:', message);
    }
  }

  async stopConnection(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
    this.stopHeartbeat();
  }

  async restartConnection(): Promise<void> {
    await this.stopConnection();
    await this.startConnection();
  }

  private registerEvent(eventName: string): void {
    if (!this.connection || this.registeredEvents.has(eventName)) return;
    this.connection.on(eventName, (payload: unknown) => {
      if (!this.eventSubjects.has(eventName)) {
        this.eventSubjects.set(eventName, new Subject<unknown>());
      }
      this.eventSubjects.get(eventName)!.next(payload);
    });
    this.registeredEvents.add(eventName);
  }

  private startHeartbeat(conn: signalR.HubConnection): void {
    this.stopHeartbeat();
    this.heartbeatInterval = setInterval(() => {
      conn.invoke('Heartbeat').catch((err: Error) => console.error('Heartbeat failed:', err.message));
    }, 30000);
  }

  private stopHeartbeat(): void {
    if (this.heartbeatInterval) {
      clearInterval(this.heartbeatInterval);
      this.heartbeatInterval = null;
    }
  }
}
