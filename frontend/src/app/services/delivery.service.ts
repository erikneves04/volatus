import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Delivery } from '../models/delivery.model';
import { ApiResponse, DeliveryViewModel, DeliveryInsertViewModel, DeliveryUpdateViewModel, DeliveryAssignmentViewModel } from '../models/api-response.model';

function getApiUrl() {
  return 'http://localhost:8081/api/delivery';
}

@Injectable({ providedIn: 'root' })
export class DeliveryService {
  private apiUrl = getApiUrl();

  constructor(private http: HttpClient) {}

  getAll(): Observable<Delivery[]> {
    return this.http.get<ApiResponse<DeliveryViewModel[]>>(this.apiUrl).pipe(
      map(response => response.data.map(this.mapDeliveryViewModelToDelivery))
    );
  }

  getById(id: string): Observable<Delivery> {
    return this.http.get<ApiResponse<DeliveryViewModel>>(`${this.apiUrl}/${id}`).pipe(
      map(response => this.mapDeliveryViewModelToDelivery(response.data))
    );
  }

  create(delivery: Partial<Delivery>): Observable<Delivery> {
    const deliveryInsert: DeliveryInsertViewModel = {
      customerName: delivery.customerName || '',
      customerAddress: delivery.customerAddress || '',
      customerPhone: delivery.customerPhone || '',
      description: delivery.description || '',
      weight: delivery.weight || 0,
      status: delivery.status || 'Pending',
      scheduledDate: delivery.scheduledDate,
      deliveredDate: delivery.deliveredDate,
      notes: delivery.notes,
      droneId: delivery.droneId
    };

    return this.http.post<ApiResponse<DeliveryViewModel>>(this.apiUrl, deliveryInsert).pipe(
      map(response => this.mapDeliveryViewModelToDelivery(response.data))
    );
  }

  update(id: string, delivery: Partial<Delivery>): Observable<Delivery> {
    const deliveryUpdate: DeliveryUpdateViewModel = {
      customerName: delivery.customerName || '',
      customerAddress: delivery.customerAddress || '',
      customerPhone: delivery.customerPhone || '',
      description: delivery.description || '',
      weight: delivery.weight || 0,
      status: delivery.status || 'Pending',
      scheduledDate: delivery.scheduledDate,
      deliveredDate: delivery.deliveredDate,
      notes: delivery.notes,
      droneId: delivery.droneId
    };

    return this.http.put<ApiResponse<DeliveryViewModel>>(`${this.apiUrl}/${id}`, deliveryUpdate).pipe(
      map(response => this.mapDeliveryViewModelToDelivery(response.data))
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`).pipe(
      map(() => void 0)
    );
  }

  assignDrone(deliveryId: string, droneId: string): Observable<Delivery> {
    const assignment: DeliveryAssignmentViewModel = {
      deliveryId: deliveryId,
      droneId: droneId
    };

    return this.http.post<ApiResponse<DeliveryViewModel>>(`${this.apiUrl}/assign-drone`, assignment).pipe(
      map(response => this.mapDeliveryViewModelToDelivery(response.data))
    );
  }

  private mapDeliveryViewModelToDelivery(deliveryViewModel: DeliveryViewModel): Delivery {
    return {
      id: deliveryViewModel.id,
      customerName: deliveryViewModel.customerName,
      customerAddress: deliveryViewModel.customerAddress,
      customerPhone: deliveryViewModel.customerPhone,
      description: deliveryViewModel.description,
      weight: deliveryViewModel.weight,
      status: deliveryViewModel.status,
      scheduledDate: deliveryViewModel.scheduledDate,
      deliveredDate: deliveryViewModel.deliveredDate,
      notes: deliveryViewModel.notes,
      droneId: deliveryViewModel.droneId,
      createdAt: deliveryViewModel.createdAt,
      updatedAt: deliveryViewModel.updatedAt
    };
  }
} 