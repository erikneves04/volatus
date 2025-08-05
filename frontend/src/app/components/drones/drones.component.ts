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
import { DroneFormDialogComponent } from '../dialogs/form-dialog/drone-form-dialog.component';
import { Drone } from '../../models/drone.model';
import { DroneService } from '../../services/drone.service';

@Component({
  selector: 'app-drones',
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
  templateUrl: './drones.component.html',
  styleUrl: './drones.component.scss'
})
export class DronesComponent implements OnInit {
  displayedColumns1: string[] = ['name', 'serialNumber', 'maxWeight', 'battery', 'status', 'actions'];
  dataSource1: Drone[] = [];
  loading = false;

  constructor(private dialog: MatDialog, private droneService: DroneService) {}

  ngOnInit(): void {
    this.loadDrones();
  }

  loadDrones(): void {
    this.loading = true;
    this.droneService.getAll().subscribe({
      next: (drones) => {
        this.dataSource1 = drones || [];
        this.loading = false;
      },
      error: (err) => {
        console.error('Erro ao carregar drones', err);
        this.dataSource1 = [];
        this.loading = false;
      }
    });
  }

  openAddDroneDialog(): void {
    const dialogRef = this.dialog.open(DroneFormDialogComponent, {
      width: '500px',
      data: null
    });

    dialogRef.afterClosed().subscribe((result: Partial<Drone>) => {
      if (result) {
        this.droneService.create(result).subscribe({
          next: () => this.loadDrones(),
          error: (err) => console.error('Erro ao criar drone', err)
        });
      }
    });
  }

  openEditDroneDialog(drone: Drone): void {
    const dialogRef = this.dialog.open(DroneFormDialogComponent, {
      width: '500px',
      data: drone
    });

    dialogRef.afterClosed().subscribe((result: Partial<Drone>) => {
      if (result && drone.id) {
        this.droneService.update(drone.id, result).subscribe({
          next: () => this.loadDrones(),
          error: (err) => console.error('Erro ao atualizar drone', err)
        });
      }
    });
  }

  deleteDrone(drone: Drone): void {
    if (drone.id) {
      this.droneService.delete(drone.id).subscribe({
        next: () => this.loadDrones(),
        error: (err) => console.error('Erro ao deletar drone', err)
      });
    }
  }
}
