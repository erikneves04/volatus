import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MaterialModule } from 'src/app/material.module';
import { DashboardService, DroneStatus } from '../../services/dashboard.service';
import { DronePositionMapComponent } from '../drone-position-map/drone-position-map.component';

@Component({
  selector: 'app-dashboard-map',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MaterialModule,
    DronePositionMapComponent
  ],
  templateUrl: './dashboard-map.component.html',
  styleUrls: ['./dashboard-map.component.scss']
})
export class DashboardMapComponent implements OnInit, OnDestroy {
  drones: DroneStatus[] = [];

  private autoRefreshTimer: any;
  private readonly REFRESH_INTERVAL = 3000; // 3 segundos

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadDashboardData();
    this.startAutoRefresh();
  }

  ngOnDestroy(): void {
    this.stopAutoRefresh();
  }

  startAutoRefresh(): void {
    this.autoRefreshTimer = setInterval(() => {
      this.loadDashboardData();
    }, this.REFRESH_INTERVAL);
  }

  stopAutoRefresh(): void {
    if (this.autoRefreshTimer) {
      clearInterval(this.autoRefreshTimer);
      this.autoRefreshTimer = null;
    }
  }

  loadDashboardData(): void {
    this.dashboardService.getDroneStatus().subscribe(drones => this.drones = drones);
  }

  refreshData(): void {
    this.loadDashboardData();
  }
} 