import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Event } from '../models/event.model';
import { ApiResponse, EventViewModel } from '../models/api-response.model';

function getApiUrl() {
  return 'http://localhost:8081/api/event';
}

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private apiUrl = getApiUrl();

  constructor(private http: HttpClient) { }

  getRecentEvents(count: number = 50): Observable<Event[]> {
    return this.http.get<ApiResponse<EventViewModel[]>>(`${this.apiUrl}?count=${count}`).pipe(
      map(response => response.data.map(this.mapEventViewModelToEvent))
    );
  }

  createEvent(title: string, description: string): Observable<Event> {
    return this.http.post<ApiResponse<EventViewModel>>(this.apiUrl, { title, description }).pipe(
      map(response => this.mapEventViewModelToEvent(response.data))
    );
  }

  private mapEventViewModelToEvent(eventViewModel: EventViewModel): Event {
    return {
      id: eventViewModel.id,
      title: eventViewModel.title,
      description: eventViewModel.description,
      createdAt: eventViewModel.createdAt
    };
  }
} 