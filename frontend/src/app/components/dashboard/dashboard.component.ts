import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MaterialModule } from 'src/app/material.module';

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
  template: `
    <div class="dashboard-container">
      <!-- Header do Dashboard -->
      <div class="dashboard-header">
        <mat-card class="header-card">
          <mat-card-content>
            <div class="d-flex justify-content-between align-items-center">
              <h2>Dashboard de Entregas</h2>
              <div class="header-actions">
                <button 
                  mat-raised-button 
                  color="primary"
                  (click)="refreshData()"
                >
                  <mat-icon>refresh</mat-icon>
                  Atualizar Dados
                </button>
              </div>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Métricas principais -->
      <div class="metrics-grid">
        <mat-card class="metric-card">
          <mat-card-content>
            <div class="metric-item">
              <div class="metric-icon">
                <mat-icon>local_shipping</mat-icon>
              </div>
              <div class="metric-content">
                <h3>{{ metrics.totalDeliveries }}</h3>
                <p>Total de Entregas</p>
              </div>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="metric-card">
          <mat-card-content>
            <div class="metric-item">
              <div class="metric-icon success">
                <mat-icon>check_circle</mat-icon>
              </div>
              <div class="metric-content">
                <h3>{{ metrics.completedDeliveries }}</h3>
                <p>Entregas Concluídas</p>
              </div>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="metric-card">
          <mat-card-content>
            <div class="metric-item">
              <div class="metric-icon warning">
                <mat-icon>schedule</mat-icon>
              </div>
              <div class="metric-content">
                <h3>{{ formatTime(metrics.averageDeliveryTime) }}</h3>
                <p>Tempo Médio por Entrega</p>
              </div>
            </div>
          </mat-card-content>
        </mat-card>

        <mat-card class="metric-card">
          <mat-card-content>
            <div class="metric-item">
              <div class="metric-icon info">
                <mat-icon>trending_up</mat-icon>
              </div>
              <div class="metric-content">
                <h3>{{ metrics.deliverySuccessRate.toFixed(1) }}%</h3>
                <p>Taxa de Sucesso</p>
              </div>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Conteúdo principal -->
      <div class="main-content">
        <!-- Status dos Drones -->
        <mat-card class="detail-card">
          <mat-card-header>
            <mat-card-title>Status dos Drones</mat-card-title>
            <mat-card-subtitle>Informações em tempo real dos drones</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <div class="drone-list" *ngIf="drones.length > 0; else noDrones">
              <div class="drone-item" *ngFor="let drone of drones">
                <div class="drone-info">
                  <h4>{{ drone.name }}</h4>
                  <p>Posição: ({{ drone.currentPosition.x.toFixed(1) }}, {{ drone.currentPosition.y.toFixed(1) }})</p>
                  <p>Entregas: {{ drone.totalDeliveries }} | Distância: {{ drone.totalDistance.toFixed(1) }}km</p>
                </div>
                <div class="drone-status">
                  <mat-chip [color]="getStatusColor(drone.status)" selected>
                    {{ getStatusLabel(drone.status) }}
                  </mat-chip>
                  <mat-progress-bar 
                    mode="determinate" 
                    [value]="drone.battery"
                    [color]="drone.battery < 20 ? 'warn' : 'primary'"
                  ></mat-progress-bar>
                  <span class="battery-text">{{ drone.battery }}%</span>
                </div>
              </div>
            </div>
            <ng-template #noDrones>
              <div class="empty-state">
                <mat-icon>drone</mat-icon>
                <p>Nenhum drone disponível no momento</p>
              </div>
            </ng-template>
          </mat-card-content>
        </mat-card>

        <!-- Entregas Recentes -->
        <mat-card class="detail-card">
          <mat-card-header>
            <mat-card-title>Entregas Recentes</mat-card-title>
            <mat-card-subtitle>Últimas entregas realizadas</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <div class="deliveries-list" *ngIf="recentDeliveries.length > 0; else noDeliveries">
              <div class="delivery-item" *ngFor="let delivery of recentDeliveries">
                <div class="delivery-info">
                  <h4>Entrega #{{ delivery.id }}</h4>
                  <p>De: ({{ delivery.origin.x.toFixed(1) }}, {{ delivery.origin.y.toFixed(1) }})</p>
                  <p>Para: ({{ delivery.destination.x.toFixed(1) }}, {{ delivery.destination.y.toFixed(1) }})</p>
                </div>
                <div class="delivery-status">
                  <mat-chip [color]="getDeliveryStatusColor(delivery.status)" selected>
                    {{ getDeliveryStatusLabel(delivery.status) }}
                  </mat-chip>
                  <span class="delivery-time">{{ formatTime(delivery.deliveryTime) }}</span>
                </div>
              </div>
            </div>
            <ng-template #noDeliveries>
              <div class="empty-state">
                <mat-icon>local_shipping</mat-icon>
                <p>Nenhuma entrega realizada ainda</p>
              </div>
            </ng-template>
          </mat-card-content>
        </mat-card>

        <!-- Log de Eventos -->
        <mat-card class="detail-card">
          <mat-card-header>
            <mat-card-title>Log de Eventos</mat-card-title>
            <mat-card-subtitle>Histórico de atividades do sistema</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <div class="events-log" *ngIf="events.length > 0; else noEvents">
              <div class="event-item" *ngFor="let event of events.slice(0, 10)">
                <div class="event-time">{{ formatEventTime(event.timestamp) }}</div>
                <div class="event-description">{{ event.description }}</div>
              </div>
            </div>
            <ng-template #noEvents>
              <div class="empty-state">
                <mat-icon>event_note</mat-icon>
                <p>Nenhum evento registrado</p>
              </div>
            </ng-template>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-container {
      padding: 20px;
      background-color: #f5f5f5;
      min-height: 100vh;
    }

    .dashboard-header {
      margin-bottom: 20px;
    }

    .header-card {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
    }

    .header-actions {
      display: flex;
      gap: 10px;
    }

    .metrics-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 20px;
      margin-bottom: 20px;
    }

    .metric-card {
      transition: transform 0.2s;
    }

    .metric-card:hover {
      transform: translateY(-2px);
    }

    .metric-item {
      display: flex;
      align-items: center;
      gap: 15px;
    }

    .metric-icon {
      width: 50px;
      height: 50px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #e3f2fd;
      color: #1976d2;
    }

    .metric-icon.success {
      background: #e8f5e8;
      color: #4caf50;
    }

    .metric-icon.warning {
      background: #fff3e0;
      color: #ff9800;
    }

    .metric-icon.info {
      background: #e1f5fe;
      color: #00bcd4;
    }

    .metric-content h3 {
      margin: 0;
      font-size: 2rem;
      font-weight: bold;
    }

    .metric-content p {
      margin: 0;
      color: #666;
    }

    .main-content {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
      gap: 20px;
    }

    .detail-card {
      height: fit-content;
    }

    .drone-list, .deliveries-list {
      display: flex;
      flex-direction: column;
      gap: 15px;
    }

    .drone-item, .delivery-item {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 15px;
      border: 1px solid #eee;
      border-radius: 8px;
      background: #fafafa;
      transition: background-color 0.2s;
    }

    .drone-item:hover, .delivery-item:hover {
      background: #f0f0f0;
    }

    .drone-info h4, .delivery-info h4 {
      margin: 0 0 5px 0;
      color: #333;
    }

    .drone-info p, .delivery-info p {
      margin: 0;
      font-size: 0.9rem;
      color: #666;
    }

    .drone-status, .delivery-status {
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      gap: 5px;
    }

    .battery-text, .delivery-time {
      font-size: 0.8rem;
      color: #666;
    }

    .events-log {
      max-height: 300px;
      overflow-y: auto;
    }

    .event-item {
      padding: 10px;
      border-bottom: 1px solid #eee;
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .event-time {
      font-size: 0.8rem;
      color: #666;
      font-weight: bold;
    }

    .event-description {
      font-size: 0.9rem;
      color: #333;
    }

    .empty-state {
      text-align: center;
      padding: 40px 20px;
      color: #666;
    }

    .empty-state mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 10px;
      color: #ccc;
    }

    .empty-state p {
      margin: 0;
      font-size: 1rem;
    }

    @media (max-width: 768px) {
      .main-content {
        grid-template-columns: 1fr;
      }
      
      .metrics-grid {
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      }
    }
  `]
})
export class DashboardComponent implements OnInit {
  // Dados que virão do backend
  drones: any[] = [];
  recentDeliveries: any[] = [];
  events: any[] = [];
  metrics: any = {
    totalDeliveries: 0,
    completedDeliveries: 0,
    averageDeliveryTime: 0,
    deliverySuccessRate: 0
  };

  constructor() {}

  ngOnInit(): void {
    // Aqui será feita a conexão com o backend
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    // TODO: Implementar chamadas para o backend
    // this.dashboardService.getDrones().subscribe(drones => this.drones = drones);
    // this.dashboardService.getDeliveries().subscribe(deliveries => this.recentDeliveries = deliveries);
    // this.dashboardService.getEvents().subscribe(events => this.events = events);
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

  formatEventTime(timestamp: Date): string {
    return timestamp.toLocaleTimeString();
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