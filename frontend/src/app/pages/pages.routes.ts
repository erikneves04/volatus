import { Routes } from '@angular/router';
import { DashboardComponent } from '../components/dashboard/dashboard.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    data: {
      title: 'Dashboard de Simulação',
      urls: [
        { title: 'Dashboard', url: '/dashboard' },
        { title: 'Simulação' },
      ],
    },
  },
];
