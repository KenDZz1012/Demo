import { Component } from '@angular/core';

@Component({
  selector: 'app-loading-screen',
  standalone: true,
  template: `
    <div class="loading-container">
      <img src="/logo.svg" alt="Logo" class="rotating-logo" />
    </div>
  `,
  styles: [`
    .loading-container {
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
      background-color: #0e0e0e;
    }
    .rotating-logo {
      width: 120px;
      height: 200px;
      animation: rotate 0.75s linear infinite;
    }
    @keyframes rotate {
      from { transform: rotate(0deg); }
      to { transform: rotate(360deg); }
    }
  `],
})
export class LoadingScreenComponent {}
