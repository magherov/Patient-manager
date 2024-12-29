import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, LoginCredential } from '../../services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export default class LoginPageComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  readonly loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  errorMessage: string = '';
  isLoading: boolean = false;

  async onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      try {
        const LoginCredential: LoginCredential = { password: this.loginForm.value.password!, username: this.loginForm.value.username! };
        const success = await this.authService.login(LoginCredential);
        if (success) {
          await this.router.navigate(['/patient-list']);
        } else {
          this.errorMessage = 'Credenziali non valide';
        }
      } catch (error) {
        this.errorMessage = 'Errore durante il login';
      } finally {
        this.isLoading = false;
      }
    }
  }
}
