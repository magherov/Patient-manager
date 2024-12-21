import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'patient-list',
    pathMatch: 'full',
  },
  {
    path: 'patient-list',
    loadComponent: () => import('./components/patient-list/patient-list.component'),
  },
  {
    path: '**',
    redirectTo: 'patient-list',
    pathMatch: 'full',
  },
];
