import { Routes } from '@angular/router';
import { authGuard } from './guard/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'patient-list',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login-page/login-page.component'),
  },
  {
    path: 'patient-list',
    loadComponent: () => import('./components/patient-list/patient-list.component'),
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: 'patient-list',
    pathMatch: 'full',
  },
];
