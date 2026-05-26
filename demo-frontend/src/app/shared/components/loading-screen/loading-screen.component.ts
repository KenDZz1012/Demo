import { Component } from '@angular/core';

@Component({
  selector: 'app-loading-screen',
  standalone: true,
  template: `
    <div class="loading-container">
      <img src="/logo.svg" alt="KenVerse" class="rotating-logo" />
      <span class="loading-container__text">Loading KenVerse...</span>
    </div>
  `,
})
export class LoadingScreenComponent {}
