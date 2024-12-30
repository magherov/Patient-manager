import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { enviroment } from '../../enviroments/enviroment';
import { firstValueFrom, lastValueFrom } from 'rxjs';
import { Router } from '@angular/router';

export interface LoginCredential {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
  expiration: Date;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly authPath = `${enviroment.apiBasePath}/Auth/login`;
  private readonly router = inject(Router);

  async login (credential: LoginCredential) {
    try {
      const result = await firstValueFrom(this.http.post<LoginResponse>(this.authPath, credential));

      if (!result) {
        return false;
      }

      localStorage.setItem('token', result.token);
      localStorage.setItem('currentUser', result.username);
      return true;
    } catch {
      return false;
    }
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');

    void this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token;
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getUser() {
    return localStorage.getItem('currentUser');
  }
}
