import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  template: `
    <div class="home">
      <h1>Chào mừng đến với KenVerse</h1>
      <a routerLink="/login" class="login-btn">Đăng nhập</a>
    </div>
  `,
  styles: [`
    .home {
      background-color: #5865F2;
      height: 100vh;
      color: white;
      display: flex;
      flex-direction: column;
      justify-content: center;
      align-items: center;
      text-align: center;
    }
    h1 { font-size: 3rem; margin-bottom: 1.5rem; }
    .login-btn {
      padding: 0.75rem 1.5rem;
      font-size: 1rem;
      background-color: white;
      color: #5865F2;
      border: none;
      border-radius: 9999px;
      cursor: pointer;
      text-decoration: none;
    }
  `],
})
export class HomeComponent {}
