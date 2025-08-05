import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MaterialModule } from 'src/app/material.module';

export interface Drone {
  id: string;
  name: string;
  weight: number;
  maxDistance: number;
  status: 'idle' | 'in-flight' | 'returning';
}

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
            <mat-label>Peso Máximo (kg)</mat-label>
            <input matInput type="number" formControlName="weight" placeholder="Ex: 5">
            <mat-error *ngIf="droneForm.get('weight')?.hasError('required')">
              Peso é obrigatório
            </mat-error>
            <mat-error *ngIf="droneForm.get('weight')?.hasError('min')">
              Peso deve ser maior que 0
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Distância Máxima (km)</mat-label>
            <input matInput type="number" formControlName="maxDistance" placeholder="Ex: 50">
            <mat-error *ngIf="droneForm.get('maxDistance')?.hasError('required')">
              Distância é obrigatória
            </mat-error>
            <mat-error *ngIf="droneForm.get('maxDistance')?.hasError('min')">
              Distância deve ser maior que 0
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Status</mat-label>
            <mat-select formControlName="status">
              <mat-option value="idle">Ocioso</mat-option>
              <mat-option value="in-flight">Em Voo</mat-option>
              <mat-option value="returning">Retornando</mat-option>
            </mat-select>
            <mat-error *ngIf="droneForm.get('status')?.hasError('required')">
              Status é obrigatório
            </mat-error>
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
export class DroneFormDialogComponent {
  droneForm: FormGroup;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DroneFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Drone | null
  ) {
    this.isEditMode = !!data;
    this.initForm();
  }

  private initForm(): void {
    this.droneForm = this.fb.group({
      name: [this.data?.name || '', [Validators.required]],
      weight: [this.data?.weight || '', [Validators.required, Validators.min(0.1)]],
      maxDistance: [this.data?.maxDistance || '', [Validators.required, Validators.min(1)]],
      status: [this.data?.status || 'idle', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.droneForm.valid) {
      const droneData: Drone = {
        id: this.data?.id || this.generateId(),
        ...this.droneForm.value
      };
      this.dialogRef.close(droneData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  private generateId(): string {
    return 'D-' + Math.random().toString(36).substr(2, 9).toUpperCase();
  }
} 