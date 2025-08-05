import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MaterialModule } from 'src/app/material.module';

export interface Delivery {
  id: string;
  name: string;
  weight: number;
  position: string;
  priority: 'high' | 'medium' | 'low';
  status: 'delivered' | 'inProgress' | 'waiting' | 'cancelled';
}

@Component({
  selector: 'app-delivery-form-dialog',
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
    <h2 mat-dialog-title>{{ isEditMode ? 'Editar Entrega' : 'Adicionar Nova Entrega' }}</h2>
    <form [formGroup]="deliveryForm" (ngSubmit)="onSubmit()">
      <mat-dialog-content>
        <div class="form-container">
          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Nome do Cliente/Pedido</mat-label>
            <input matInput formControlName="name" placeholder="Ex: Eletrônicos Express">
            <mat-error *ngIf="deliveryForm.get('name')?.hasError('required')">
              Nome é obrigatório
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Peso (kg)</mat-label>
            <input matInput type="number" formControlName="weight" placeholder="Ex: 2.5">
            <mat-error *ngIf="deliveryForm.get('weight')?.hasError('required')">
              Peso é obrigatório
            </mat-error>
            <mat-error *ngIf="deliveryForm.get('weight')?.hasError('min')">
              Peso deve ser maior que 0
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Posição (x, y)</mat-label>
            <input matInput formControlName="position" placeholder="Ex: (15, 20)">
            <mat-error *ngIf="deliveryForm.get('position')?.hasError('required')">
              Posição é obrigatória
            </mat-error>
            <mat-error *ngIf="deliveryForm.get('position')?.hasError('pattern')">
              Formato inválido. Use (x, y)
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Prioridade</mat-label>
            <mat-select formControlName="priority">
              <mat-option value="low">Baixa</mat-option>
              <mat-option value="medium">Média</mat-option>
              <mat-option value="high">Alta</mat-option>
            </mat-select>
            <mat-error *ngIf="deliveryForm.get('priority')?.hasError('required')">
              Prioridade é obrigatória
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Status</mat-label>
            <mat-select formControlName="status">
              <mat-option value="waiting">Aguardando</mat-option>
              <mat-option value="inProgress">Em Progresso</mat-option>
              <mat-option value="delivered">Entregue</mat-option>
              <mat-option value="cancelled">Cancelado</mat-option>
            </mat-select>
            <mat-error *ngIf="deliveryForm.get('status')?.hasError('required')">
              Status é obrigatório
            </mat-error>
          </mat-form-field>
        </div>
      </mat-dialog-content>
      <mat-dialog-actions align="end">
        <button mat-button type="button" (click)="onCancel()">Cancelar</button>
        <button mat-raised-button color="primary" type="submit" [disabled]="deliveryForm.invalid">
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
export class DeliveryFormDialogComponent {
  deliveryForm: FormGroup;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DeliveryFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Delivery | null
  ) {
    this.isEditMode = !!data;
    this.initForm();
  }

  private initForm(): void {
    this.deliveryForm = this.fb.group({
      name: [this.data?.name || '', [Validators.required]],
      weight: [this.data?.weight || '', [Validators.required, Validators.min(0.1)]],
      position: [this.data?.position || '', [Validators.required, Validators.pattern(/^\(-?\d+,\s*-?\d+\)$/)]],
      priority: [this.data?.priority || 'medium', [Validators.required]],
      status: [this.data?.status || 'waiting', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.deliveryForm.valid) {
      const deliveryData: Delivery = {
        id: this.data?.id || this.generateId(),
        ...this.deliveryForm.value
      };
      this.dialogRef.close(deliveryData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  private generateId(): string {
    return 'PED-' + Math.random().toString(36).substr(2, 9).toUpperCase();
  }
} 