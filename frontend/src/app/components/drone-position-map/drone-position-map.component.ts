import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DroneStatus } from '../../services/dashboard.service';

@Component({
  selector: 'app-drone-position-map',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatTooltipModule
  ],
  templateUrl: './drone-position-map.component.html',
  styleUrls: ['./drone-position-map.component.scss']
})
export class DronePositionMapComponent implements OnInit, OnChanges {
  @Input() drones: DroneStatus[] = [];
  
  // Configurações da matriz
  readonly gridHeight = 40; // Altura
  readonly gridWidth = 120; // Largura
  readonly cellSize = 15; // Reduzindo o tamanho da célula para caber melhor na tela
  
  grid: any[][] = [];
  
  ngOnInit(): void {
    this.initializeGrid();
    this.updateDronePositions();
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['drones']) {
      this.updateDronePositions();
    }
  }
  
  private initializeGrid(): void {
    this.grid = [];
    for (let i = 0; i < this.gridHeight; i++) {
      this.grid[i] = [];
      for (let j = 0; j < this.gridWidth; j++) {
        this.grid[i][j] = { type: 'empty', drone: null };
      }
    }
  }
  
  private updateDronePositions(): void {
    this.initializeGrid();
    
    // Marcar a base na posição central (0,0)
    const baseGridX = Math.floor((0 + 10) * (this.gridWidth - 1) / 20);
    const baseGridY = Math.floor((0 + 10) * (this.gridHeight - 1) / 20);
    this.grid[baseGridY][baseGridX] = {
      type: 'base',
      drone: null
    };
    
    this.drones.forEach(drone => {
      // Converter coordenadas para posições na grade
      const currentGridX = Math.floor((drone.currentX + 10) * (this.gridWidth - 1) / 20);
      const currentGridY = Math.floor((drone.currentY + 10) * (this.gridHeight - 1) / 20);
      const targetGridX = Math.floor((drone.targetX + 10) * (this.gridWidth - 1) / 20);
      const targetGridY = Math.floor((drone.targetY + 10) * (this.gridHeight - 1) / 20);
      
      // Garantir que as posições estejam dentro dos limites
      const safeCurrentX = Math.max(0, Math.min(this.gridWidth - 1, currentGridX));
      const safeCurrentY = Math.max(0, Math.min(this.gridHeight - 1, currentGridY));
      const safeTargetX = Math.max(0, Math.min(this.gridWidth - 1, targetGridX));
      const safeTargetY = Math.max(0, Math.min(this.gridHeight - 1, targetGridY));
      
      // Marcar posição atual do drone (se não for a base)
      if (safeCurrentX !== baseGridX || safeCurrentY !== baseGridY) {
        this.grid[safeCurrentY][safeCurrentX] = {
          type: 'drone',
          drone: drone
        };
      }
      
      // Marcar posição de destino (se diferente da atual e não for a base)
      if ((safeTargetX !== safeCurrentX || safeTargetY !== safeCurrentY) && 
          (safeTargetX !== baseGridX || safeTargetY !== baseGridY)) {
        this.grid[safeTargetY][safeTargetX] = {
          type: 'target',
          drone: drone
        };
      }
    });
  }
  
  getCellClass(cell: any): string {
    switch (cell.type) {
      case 'drone':
        return 'drone-cell';
      case 'target':
        return 'target-cell';
      case 'base':
        return 'base-cell';
      default:
        return 'empty-cell';
    }
  }
  
  getDroneStatusColor(status: string): string {
    const colors: { [key: string]: string } = {
      'Available': 'primary',
      'InUse': 'accent',
      'Maintenance': 'warn',
      'Offline': 'disabled'
    };
    return colors[status] || 'primary';
  }
  
  getDroneStatusLabel(status: string): string {
    const labels: { [key: string]: string } = {
      'Available': 'Disponível',
      'InUse': 'Em Uso',
      'Maintenance': 'Manutenção',
      'Offline': 'Offline'
    };
    return labels[status] || status;
  }
  
  getCellTooltip(cell: any): string {
    if (cell.type === 'base') {
      return 'Base de Operações - Posição: (0, 0)';
    } else if (cell.type === 'drone' && cell.drone) {
      return `${cell.drone.name} - Posição: (${cell.drone.currentX.toFixed(1)}, ${cell.drone.currentY.toFixed(1)}) - Bateria: ${cell.drone.batteryLevel}%`;
    } else if (cell.type === 'target' && cell.drone) {
      return `Destino de ${cell.drone.name} - (${cell.drone.targetX.toFixed(1)}, ${cell.drone.targetY.toFixed(1)})`;
    }
    return '';
  }
} 