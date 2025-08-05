import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MaterialModule } from 'src/app/material.module';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event.model';

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
    MaterialModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  // Dados que virão do backend
  drones: any[] = [];
  recentDeliveries: any[] = [];
  events: Event[] = [];
  metrics: any = {
    totalDeliveries: 0,
    completedDeliveries: 0,
    averageDeliveryTime: 0,
    deliverySuccessRate: 0
  };

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    // Aqui será feita a conexão com o backend
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    // TODO: Implementar chamadas para o backend
    // this.dashboardService.getDrones().subscribe(drones => this.drones = drones);
    // this.dashboardService.getDeliveries().subscribe(deliveries => this.recentDeliveries = deliveries);
    this.eventService.getRecentEvents(10).subscribe(events => this.events = events);
    // this.dashboardService.getMetrics().subscribe(metrics => this.metrics = metrics);
  }

  refreshData(): void {
    this.loadDashboardData();
  }

  formatTime(milliseconds: number): string {
    if (milliseconds === 0) return '0s';
    const seconds = Math.floor(milliseconds / 1000);
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    
    if (minutes > 0) {
      return `${minutes}m ${remainingSeconds}s`;
    }
    return `${seconds}s`;
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