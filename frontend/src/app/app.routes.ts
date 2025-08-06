import { Routes } from '@angular/router';
import { BlankComponent } from './layouts/blank/blank.component';
import { FullComponent } from './layouts/full/full.component';
import { DeliveriesComponent } from './components/deliveries/deliveries.component';
import { DronesComponent } from './components/drones/drones.component';
import { DashboardStatsComponent } from './components/dashboard-stats/dashboard-stats.component';
import { DashboardMapComponent } from './components/dashboard-map/dashboard-map.component';

export const routes: Routes = [
  {
    path: '',
    component: FullComponent,
    children: [
      {
        path: '',
        redirectTo: '/dashboard-stats',
        pathMatch: 'full',
      },
      {
        path: 'dashboard-stats',
        component: DashboardStatsComponent
      },
      {
        path: 'dashboard-map',
        component: DashboardMapComponent
      },
      {
        path: 'deliveries',
        component: DeliveriesComponent
      },
      {
        path: 'drones',
        component: DronesComponent
      },
      {
        path: 'ui-components',
        loadChildren: () =>
          import('./pages/ui-components/ui-components.routes').then(
            (m) => m.UiComponentsRoutes
          ),
      },
      {
        path: 'extra',
        loadChildren: () =>
          import('./pages/extra/extra.routes').then((m) => m.ExtraRoutes),
      },
    ],
  },
  {
    path: '',
    component: BlankComponent,
    children: [
      {
        path: 'authentication',
        loadChildren: () =>
          import('./pages/authentication/authentication.routes').then(
            (m) => m.AuthenticationRoutes
          ),
      },
    ],
  },
  {
    path: '**',
    redirectTo: 'authentication/error',
  },
];
