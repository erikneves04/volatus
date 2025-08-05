import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Drone } from '../models/drone.model';
import { ApiResponse, DroneViewModel, DroneInsertViewModel, DroneUpdateViewModel } from '../models/api-response.model';

function getApiUrl() {
  return 'http://localhost:8081/api/drone';
}

@Injectable({ providedIn: 'root' })
export class DroneService {
  private apiUrl = getApiUrl();

  constructor(private http: HttpClient) {}

  getAll(): Observable<Drone[]> {
    return this.http.get<ApiResponse<DroneViewModel[]>>(this.apiUrl).pipe(
      map(response => response.data.map(this.mapDroneViewModelToDrone))
    );
  }

  getById(id: string): Observable<Drone> {
    return this.http.get<ApiResponse<DroneViewModel>>(`${this.apiUrl}/${id}`).pipe(
      map(response => this.mapDroneViewModelToDrone(response.data))
    );
  }

  create(drone: Partial<Drone>): Observable<Drone> {
    const droneInsert: DroneInsertViewModel = {
      name: drone.name || '',
      model: drone.model || '',
      serialNumber: drone.serialNumber || '',
      status: drone.status || '',
      maxWeight: drone.maxWeight || 0,
      batteryCapacity: drone.batteryCapacity || 0,
      currentBattery: drone.currentBattery || 0,
      notes: drone.notes
    };

    return this.http.post<ApiResponse<DroneViewModel>>(this.apiUrl, droneInsert).pipe(
      map(response => this.mapDroneViewModelToDrone(response.data))
    );
  }

  update(id: string, drone: Partial<Drone>): Observable<Drone> {
    const droneUpdate: DroneUpdateViewModel = {
      name: drone.name || '',
      model: drone.model || '',
      serialNumber: drone.serialNumber || '',
      status: drone.status || '',
      maxWeight: drone.maxWeight || 0,
      batteryCapacity: drone.batteryCapacity || 0,
      currentBattery: drone.currentBattery || 0,
      notes: drone.notes
    };

    return this.http.put<ApiResponse<DroneViewModel>>(`${this.apiUrl}/${id}`, droneUpdate).pipe(
      map(response => this.mapDroneViewModelToDrone(response.data))
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`).pipe(
      map(() => void 0)
    );
  }

  private mapDroneViewModelToDrone(droneViewModel: DroneViewModel): Drone {
    return {
      id: droneViewModel.id,
      name: droneViewModel.name,
      model: droneViewModel.model,
      serialNumber: droneViewModel.serialNumber,
      status: droneViewModel.status,
      maxWeight: droneViewModel.maxWeight,
      batteryCapacity: droneViewModel.batteryCapacity,
      currentBattery: droneViewModel.currentBattery,
      notes: droneViewModel.notes,
      createdAt: droneViewModel.createdAt,
      updatedAt: droneViewModel.updatedAt
    };
  }
}