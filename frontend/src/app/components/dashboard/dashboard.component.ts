import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MaterialModule } from 'src/app/material.module';
import { DashboardService, DashboardMetrics, DroneStatus, RecentDelivery, RecentEvent } from '../../services/dashboard.service';
import { DronePositionMapComponent } from '../drone-position-map/drone-position-map.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatProgressBarModule,
    MatChipsModule,
    MaterialModule,
    DronePositionMapComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  // Dados que virão do backend
  drones: DroneStatus[] = [];
  recentDeliveries: RecentDelivery[] = [];
  events: RecentEvent[] = [];
  metrics: DashboardMetrics = {
    totalDeliveries: 0,
    completedDeliveries: 0,
    averageDeliveryTimeSeconds: 0,
    successRate: 0
  };

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    // Aqui será feita a conexão com o backend
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    // Carregar métricas do dashboard
    this.dashboardService.getMetrics().subscribe(metrics => this.metrics = metrics);
    
    // Carregar status dos drones
    this.dashboardService.getDroneStatus().subscribe(drones => this.drones = drones);
    
    // Carregar entregas recentes
    this.dashboardService.getRecentDeliveries(5).subscribe(deliveries => this.recentDeliveries = deliveries);
    
    // Carregar eventos recentes
    this.dashboardService.getRecentEvents(10).subscribe(events => this.events = events);
  }

  refreshData(): void {
    this.loadDashboardData();
  }

  formatTime(seconds: number): string {
    if (seconds === 0) return '0s';
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    
    if (minutes > 0) {
      return `${minutes}m ${remainingSeconds}s`;
    }
    return `${Math.floor(seconds)}s`;
  }

  formatEventTime(timestamp: string): string {
    return new Date(timestamp).toLocaleTimeString();
  }

  getStatusColor(status: string): string {
    const colors: { [key: string]: string } = {
      'idle': 'primary',
      'loading': 'accent',
      'in-flight': 'warn',
      'delivering': 'warn',
      'returning': 'accent'
    };
    return colors[status] || 'primary';
  }

  getStatusLabel(status: string): string {
    const labels: { [key: string]: string } = {
      'idle': 'Ocioso',
      'loading': 'Carregando',
      'in-flight': 'Em Voo',
      'delivering': 'Entregando',
      'returning': 'Retornando'
    };
    return labels[status] || status;
  }

  getDeliveryStatusColor(status: string): string {
    const colors: { [key: string]: string } = {
      'pending': 'warn',
      'in-progress': 'accent',
      'completed': 'primary',
      'failed': 'warn'
    };
    return colors[status] || 'primary';
  }

  getDeliveryStatusLabel(status: string): string {
    const labels: { [key: string]: string } = {
      'pending': 'Pendente',
      'in-progress': 'Em Andamento',
      'completed': 'Concluída',
      'failed': 'Falhou'
    };
    return labels[status] || status;
  }
} 