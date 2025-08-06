import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Inicio',
  },
  {
    displayName: 'Dashboard',
    iconName: 'layout-grid-add',
    route: '/dashboard',
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
