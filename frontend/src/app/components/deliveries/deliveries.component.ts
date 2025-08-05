import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MaterialModule } from 'src/app/material.module';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';

// table 1
export interface Delivery {
  id: string;
  name: string;       // Nome do cliente ou pedido
  weight: number;     // Peso em kg
  position: string;   // Coordenadas (x, y)
  priority: 'high' | 'medium' | 'low';
  status: 'delivered' | 'inProgress' | 'waiting' | 'cancelled';
}

const ELEMENT_DATA: Delivery[] = [
  { id: 'PED-001', name: 'Eletrônicos Express', weight: 2.5, position: '(15, 20)', priority: 'high', status: 'inProgress' },
  { id: 'PED-002', name: 'Farmácia Central', weight: 0.8, position: '(5, -10)', priority: 'medium', status: 'waiting' },
  { id: 'PED-003', name: 'Supermercado Boas Compras', weight: 5.0, position: '(30, 45)', priority: 'low', status: 'waiting' },
  { id: 'PED-004', name: 'Restaurante Sabor Divino', weight: 1.2, position: '(-8, 3)', priority: 'high', status: 'delivered' },
  { id: 'PED-005', name: 'Livraria Cultura', weight: 3.1, position: '(-12, -15)', priority: 'medium', status: 'cancelled' },
  { id: 'PED-006', name: 'Pet Shop Amigo Fiel', weight: 4.2, position: '(22, 5)', priority: 'low', status: 'waiting' },
  { id: 'PED-007', name: 'Loja de Roupas Estilo', weight: 1.8, position: '(0, 18)', priority: 'medium', status: 'delivered' },
  { id: 'PED-008', name: 'Oficina Mecânica Rápida', weight: 6.5, position: '(-30, -10)', priority: 'low', status: 'inProgress' },
  { id: 'PED-009', name: 'Padaria Pão Quente', weight: 0.5, position: '(2, 2)', priority: 'high', status: 'waiting' },
  { id: 'PED-010', name: 'Floricultura Jardim Secreto', weight: 1.5, position: '(10, 25)', priority: 'medium', status: 'delivered' },
  { id: 'PED-011', name: 'Materiais de Construção Lar', weight: 10.0, position: '(50, 0)', priority: 'low', status: 'waiting' },
  { id: 'PED-012', name: 'Pizzaria Noite Feliz', weight: 2.0, position: '(-5, 12)', priority: 'high', status: 'inProgress' },
  { id: 'PED-013', name: 'Doceria Doce Sonho', weight: 0.7, position: '(8, 8)', priority: 'medium', status: 'cancelled' },
  { id: 'PED-014', name: 'Artigos Esportivos Campeão', weight: 3.8, position: '(18, -22)', priority: 'low', status: 'delivered' },
  { id: 'PED-015', name: 'Papelaria Criativa', weight: 1.1, position: '(-15, 20)', priority: 'medium', status: 'waiting' },
];

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
  ],
  templateUrl: './deliveries.component.html',
  styleUrl: './deliveries.component.scss'
})
export class DeliveriesComponent {
  // table 1
  displayedColumns1: string[] = ['name', 'weight', 'position', 'priority', 'status'];
  dataSource1 = ELEMENT_DATA;
}
