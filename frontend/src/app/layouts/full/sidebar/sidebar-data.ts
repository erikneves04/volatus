import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Inicio',
  },
  {
    displayName: 'Dashboard - Estatísticas',
    iconName: 'layout-grid-add',
    route: '/dashboard-stats',
  },
  {
    displayName: 'Dashboard - Mapa',
    iconName: 'map',
    route: '/dashboard-map',
  },
  {
    navCap: 'Gerenciamento',
  },
  {
    displayName: 'Entregas',
    iconName: 'truck-delivery',
    route: '/deliveries',
  },
  {
    displayName: 'Drones',
    iconName: 'plane',
    route: '/drones',
  }
];
