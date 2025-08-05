import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Drone } from '../models/drone.model';

function getApiUrl() {
  // Em produção, o backend pode injetar window['API_URL'] via script
  // Em dev, usa proxy /api
  return (window as any)['API_URL'] ? `${(window as any)['API_URL']}/drone` : '/api/drone';
}

@Injectable({ providedIn: 'root' })
export class DroneService {
  private apiUrl = getApiUrl();

  constructor(private http: HttpClient) {}

  getAll(): Observable<Drone[]> {
    return this.http.get<Drone[]>(this.apiUrl);
  }

  getById(id: string): Observable<Drone> {
    return this.http.get<Drone>(`${this.apiUrl}/${id}`);
  }

  create(drone: Partial<Drone>): Observable<Drone> {
    return this.http.post<Drone>(this.apiUrl, drone);
  }

  update(id: string, drone: Partial<Drone>): Observable<Drone> {
    return this.http.put<Drone>(`${this.apiUrl}/${id}`, drone);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}