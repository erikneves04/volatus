import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Drone } from '../models/drone.model';

function getApiUrl() {
  return 'http://localhost:8081/api/drone';
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