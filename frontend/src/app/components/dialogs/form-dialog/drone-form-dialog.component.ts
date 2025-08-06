import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MaterialModule } from 'src/app/material.module';
import { Drone } from '../../../models/drone.model';

@Component({
  selector: 'app-drone-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MaterialModule
  ],
  template: `
    <h2 mat-dialog-title>{{ isEditMode ? 'Editar Drone' : 'Adicionar Novo Drone' }}</h2>
    <form [formGroup]="droneForm" (ngSubmit)="onSubmit()">
      <mat-dialog-content>
        <div class="form-container">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Nome do Drone</mat-label>
            <input matInput formControlName="name" placeholder="Ex: Zangão Alfa">
            <mat-error *ngIf="droneForm.get('name')?.hasError('required')">
              Nome é obrigatório
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Modelo</mat-label>
            <input matInput formControlName="model" placeholder="Ex: DJI Mavic">
            <mat-error *ngIf="droneForm.get('model')?.hasError('required')">
              Modelo é obrigatório
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Número de Série</mat-label>
            <input matInput formControlName="serialNumber" placeholder="Ex: SN123456">
            <mat-error *ngIf="droneForm.get('serialNumber')?.hasError('required')">
              Número de série é obrigatório
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Status</mat-label>
            <mat-select formControlName="status">
              <mat-option value="Disponível">Disponível</mat-option>
              <mat-option value="Em Uso">Em uso</mat-option>
              <mat-option value="Maintenance">Manutenção</mat-option>
              <mat-option value="Offline">Offline</mat-option>
            </mat-select>
            <mat-error *ngIf="droneForm.get('status')?.hasError('required')">
              Status é obrigatório
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Peso Máximo (kg)</mat-label>
            <input matInput type="number" formControlName="maxWeight" placeholder="Ex: 5">
            <mat-error *ngIf="droneForm.get('maxWeight')?.hasError('required')">
              Peso é obrigatório
            </mat-error>
            <mat-error *ngIf="droneForm.get('maxWeight')?.hasError('min')">
              Peso deve ser maior que 0
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Capacidade da Bateria</mat-label>
            <input matInput type="number" formControlName="batteryCapacity" placeholder="Ex: 60">
            <mat-error *ngIf="droneForm.get('batteryCapacity')?.hasError('required')">
              Capacidade é obrigatória
            </mat-error>
            <mat-error *ngIf="droneForm.get('batteryCapacity')?.hasError('min')">
              Capacidade deve ser maior que 0
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Bateria Atual (%)</mat-label>
            <input matInput type="number" formControlName="currentBattery" placeholder="Ex: 100">
            <mat-error *ngIf="droneForm.get('currentBattery')?.hasError('required')">
              Bateria atual é obrigatória
            </mat-error>
            <mat-error *ngIf="droneForm.get('currentBattery')?.hasError('min') || droneForm.get('currentBattery')?.hasError('max')">
              Valor deve ser entre 0 e 100
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Observações</mat-label>
            <textarea matInput formControlName="notes" placeholder="Observações"></textarea>
          </mat-form-field>
        </div>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="onCancel()">Cancelar</button>
        <button mat-raised-button color="primary" type="submit" [disabled]="droneForm.invalid">
          {{ isEditMode ? 'Atualizar' : 'Adicionar' }}
        </button>
      </mat-dialog-actions>
    </form>
  `,
  styles: [`
    .form-container {
      display: flex;
      flex-direction: column;
      gap: 16px;
      min-width: 400px;
    }
    .full-width {
      width: 100%;
    }
  `]
})
export class DroneFormDialogComponent implements OnInit {
  droneForm!: FormGroup;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DroneFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Drone | null
  ) {
    this.isEditMode = !!data;
  }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.droneForm = this.fb.group({
      name: [this.data?.name || '', [Validators.required]],
      model: [this.data?.model || '', [Validators.required]],
      serialNumber: [this.data?.serialNumber || '', [Validators.required]],
      status: [this.data?.status || 'Disponível', [Validators.required]],
      maxWeight: [this.data?.maxWeight || null, [Validators.required, Validators.min(0.1)]],
      batteryCapacity: [this.data?.batteryCapacity || null, [Validators.required, Validators.min(1)]],
      currentBattery: [this.data?.currentBattery ?? 100, [Validators.required, Validators.min(0), Validators.max(100)]],
      notes: [this.data?.notes || '']
    });
  }

  onSubmit(): void {
    if (this.droneForm && this.droneForm.valid) {
      const droneData: Partial<Drone> = {
        ...this.data,
        ...this.droneForm.value
      };
      this.dialogRef.close(droneData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
} 