import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MaterialModule } from 'src/app/material.module';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { DeliveryFormDialogComponent } from '../dialogs/form-dialog/delivery-form-dialog.component';
import { Delivery } from '../../models/delivery.model';
import { DeliveryService } from '../../services/delivery.service';

@Component({
  selector: 'app-deliveries',
  imports: [
    MatTableModule,
    CommonModule,
    MatCardModule,
    MaterialModule,
    MatIconModule,
    MatMenuModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './deliveries.component.html',
  styleUrl: './deliveries.component.scss'
})
export class DeliveriesComponent implements OnInit {
  displayedColumns1: string[] = ['customerName', 'customerAddress', 'weight', 'status', 'actions'];
  dataSource1: Delivery[] = [];
  loading = false;

  constructor(private dialog: MatDialog, private deliveryService: DeliveryService) {}

  ngOnInit(): void {
    this.loadDeliveries();
  }

  loadDeliveries(): void {
    this.loading = true;
    this.deliveryService.getAll().subscribe({
      next: (deliveries) => {
        this.dataSource1 = deliveries || [];
        this.loading = false;
      },
      error: (err) => {
        console.error('Erro ao carregar entregas', err);
        this.dataSource1 = [];
        this.loading = false;
      }
    });
  }

  openAddDeliveryDialog(): void {
    const dialogRef = this.dialog.open(DeliveryFormDialogComponent, {
      width: '600px',
      data: null
    });

    dialogRef.afterClosed().subscribe((result: Partial<Delivery>) => {
      if (result) {
        this.deliveryService.create(result).subscribe({
          next: () => this.loadDeliveries(),
          error: (err) => console.error('Erro ao criar entrega', err)
        });
      }
    });
  }

  openEditDeliveryDialog(delivery: Delivery): void {
    const dialogRef = this.dialog.open(DeliveryFormDialogComponent, {
      width: '600px',
      data: delivery
    });

    dialogRef.afterClosed().subscribe((result: Partial<Delivery>) => {
      if (result && delivery.id) {
        this.deliveryService.update(delivery.id, result).subscribe({
          next: () => this.loadDeliveries(),
          error: (err) => console.error('Erro ao atualizar entrega', err)
        });
      }
    });
  }

  deleteDelivery(delivery: Delivery): void {
    if (delivery.id) {
      this.deliveryService.delete(delivery.id).subscribe({
        next: () => this.loadDeliveries(),
        error: (err) => console.error('Erro ao deletar entrega', err)
      });
    }
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Pending':
        return 'bg-light-warning text-warning';
      case 'InProgress':
        return 'bg-light-info text-info';
      case 'Delivered':
        return 'bg-light-success text-success';
      case 'Cancelled':
        return 'bg-light-error text-error';
      default:
        return 'bg-light-secondary text-secondary';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Pending':
        return 'Pendente';
      case 'InProgress':
        return 'Em Andamento';
      case 'Delivered':
        return 'Entregue';
      case 'Cancelled':
        return 'Cancelada';
      default:
        return status;
    }
  }

  formatAddress(address: string): string {
    return address || '-';
  }
}
