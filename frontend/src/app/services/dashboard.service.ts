import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';

function getApiUrl() {
  return 'http://localhost:8081/api/dashboard';
}

export interface DashboardMetrics {
  totalDeliveries: number;
  completedDeliveries: number;
  averageDeliveryTimeSeconds: number;
  successRate: number;
}

export interface DroneStatus {
  id: string;
  name: string;
  serialNumber: string;
  status: string;
  batteryLevel: number;
  lastUpdate: string;
}

export interface RecentDelivery {
  id: string;
  customerName: string;
  customerAddress: string;
  description: string;
  weight: number;
  status: string;
  priority: string;
  deliveredDate?: string;
  notes?: string;
  droneId?: string;
  createdAt: string;
  updatedAt: string;
}

export interface RecentEvent {
  id: string;
  title: string;
  description: string;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private apiUrl = getApiUrl();

  constructor(private http: HttpClient) {}

  getMetrics(): Observable<DashboardMetrics> {
    return this.http.get<ApiResponse<DashboardMetrics>>(`${this.apiUrl}/metrics`).pipe(
      map(response => response.data)
    );
  }

  getDroneStatus(): Observable<DroneStatus[]> {
    return this.http.get<ApiResponse<DroneStatus[]>>(`${this.apiUrl}/drones/status`).pipe(
      map(response => response.data)
    );
  }

  getRecentDeliveries(count: number = 5): Observable<RecentDelivery[]> {
    return this.http.get<ApiResponse<RecentDelivery[]>>(`${this.apiUrl}/deliveries/recent?count=${count}`).pipe(
      map(response => response.data)
    );
  }

  getRecentEvents(count: number = 10): Observable<RecentEvent[]> {
    return this.http.get<ApiResponse<RecentEvent[]>>(`${this.apiUrl}/events/recent?count=${count}`).pipe(
      map(response => response.data)
    );
  }
} 