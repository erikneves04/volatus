import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MaterialModule } from 'src/app/material.module';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { DroneFormDialogComponent, Drone } from '../dialogs/form-dialog/drone-form-dialog.component';



const ELEMENT_DATA: Drone[] = [
  { id: 'D-001', name: 'Zangão Alfa', weight: 5, maxDistance: 50, status: 'idle' },
  { id: 'D-002', name: 'Colibri Beta', weight: 2, maxDistance: 80, status: 'in-flight' },
  { id: 'D-003', name: 'Falcão Gama', weight: 10, maxDistance: 40, status: 'returning' },
  { id: 'D-004', name: 'Libélula Delta', weight: 1, maxDistance: 100, status: 'idle' },
  { id: 'D-005', name: 'Águia Ômega', weight: 15, maxDistance: 35, status: 'returning' },
  { id: 'D-006', name: 'Vespa Épsilon', weight: 3, maxDistance: 65, status: 'idle' },
  { id: 'D-007', name: 'Gavião Zeta', weight: 8, maxDistance: 45, status: 'in-flight' },
  { id: 'D-008', name: 'Condor Sigma', weight: 12, maxDistance: 30, status: 'returning' },
  { id: 'D-009', name: 'Andorinha Rho', weight: 2.5, maxDistance: 75, status: 'idle' },
  { id: 'D-010', name: 'Coruja Theta', weight: 4, maxDistance: 60, status: 'in-flight' },
];

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
  ],
  templateUrl: './drones.component.html',
  styleUrl: './drones.component.scss'
})
export class DronesComponent {
  // table 1
  displayedColumns1: string[] = ['name', 'weight', 'maxDistance', 'status'];
  dataSource1 = ELEMENT_DATA;

  constructor(private dialog: MatDialog) {}

  openAddDroneDialog(): void {
    const dialogRef = this.dialog.open(DroneFormDialogComponent, {
      width: '500px',
      data: null
    });

    dialogRef.afterClosed().subscribe((result: Drone) => {
      if (result) {
        this.dataSource1 = [...this.dataSource1, result];
      }
    });
  }

  openEditDroneDialog(drone: Drone): void {
    const dialogRef = this.dialog.open(DroneFormDialogComponent, {
      width: '500px',
      data: drone
    });

    dialogRef.afterClosed().subscribe((result: Drone) => {
      if (result) {
        const index = this.dataSource1.findIndex(d => d.id === result.id);
        if (index !== -1) {
          this.dataSource1[index] = result;
          this.dataSource1 = [...this.dataSource1];
        }
      }
    });
  }

  deleteDrone(drone: Drone): void {
    this.dataSource1 = this.dataSource1.filter(d => d.id !== drone.id);
  }
}
