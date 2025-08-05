import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { MaterialModule } from 'src/app/material.module';
import { Delivery } from '../../../models/delivery.model';

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
            <mat-label>Nome do Cliente</mat-label>
            <input matInput formControlName="customerName" placeholder="Ex: João Silva">
            <mat-error *ngIf="deliveryForm.get('customerName')?.hasError('required')">
              Nome do cliente é obrigatório
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Posição (x, y)</mat-label>
            <input matInput formControlName="customerAddress" placeholder="Ex: (15, 20)">
            <mat-error *ngIf="deliveryForm.get('customerAddress')?.hasError('required')">
              Posição é obrigatória
            </mat-error>
            <mat-error *ngIf="deliveryForm.get('customerAddress')?.hasError('pattern')">
              Formato inválido. Use (x, y)
            </mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Descrição da Entrega</mat-label>
            <textarea matInput formControlName="description" placeholder="Ex: Entrega de eletrônicos"></textarea>
            <mat-error *ngIf="deliveryForm.get('description')?.hasError('required')">
              Descrição é obrigatória
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
             <mat-label>Status</mat-label>
             <mat-select formControlName="status">
               <mat-option value="Pending">Pendente</mat-option>
               <mat-option value="InProgress">Em Andamento</mat-option>
               <mat-option value="Delivered">Entregue</mat-option>
               <mat-option value="Cancelled">Cancelada</mat-option>
             </mat-select>
             <mat-error *ngIf="deliveryForm.get('status')?.hasError('required')">
               Status é obrigatório
             </mat-error>
           </mat-form-field>

           <mat-form-field appearance="outline" class="full-width">
             <mat-label>Prioridade</mat-label>
             <mat-select formControlName="priority">
               <mat-option value="Low">Baixa</mat-option>
               <mat-option value="Medium">Média</mat-option>
               <mat-option value="High">Alta</mat-option>
             </mat-select>
             <mat-error *ngIf="deliveryForm.get('priority')?.hasError('required')">
               Prioridade é obrigatória
             </mat-error>
           </mat-form-field>



          <mat-form-field appearance="outline" class="full-width">
            <mat-label>Observações</mat-label>
            <textarea matInput formControlName="notes" placeholder="Observações adicionais"></textarea>
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
      min-width: 500px;
    }
    .full-width {
      width: 100%;
    }
  `]
})
export class DeliveryFormDialogComponent implements OnInit {
  deliveryForm!: FormGroup;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DeliveryFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Delivery | null
  ) {
    this.isEditMode = !!data;
  }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.deliveryForm = this.fb.group({
      customerName: [this.data?.customerName || '', [Validators.required]],
      customerAddress: [this.data?.customerAddress || '', [Validators.required, Validators.pattern(/^\(-?\d+,\s*-?\d+\)$/)]],
      description: [this.data?.description || '', [Validators.required]],
      weight: [this.data?.weight || null, [Validators.required, Validators.min(0.1)]],
      status: [this.data?.status || 'Pending', [Validators.required]],
      priority: [this.data?.priority || 'Medium', [Validators.required]],
      deliveredDate: [this.data?.deliveredDate ? new Date(this.data.deliveredDate) : null],
      notes: [this.data?.notes || '']
    });
  }

  onSubmit(): void {
    if (this.deliveryForm && this.deliveryForm.valid) {
      const formValue = this.deliveryForm.value;
      
      // Converter data para string ISO se existir
      if (formValue.deliveredDate) {
        formValue.deliveredDate = formValue.deliveredDate.toISOString();
      }

      const deliveryData: Partial<Delivery> = {
        ...this.data,
        ...formValue
      };
      this.dialogRef.close(deliveryData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
} 