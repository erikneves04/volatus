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
  readonly gridHeight = 25; // Altura ainda mais reduzida
  readonly gridWidth = 40; // Largura reduzida para melhor proporção
  readonly cellSize = 25; // Aumentando o tamanho da célula para melhor visualização
  
  grid: any[][] = [];
  
  // Marcações de escala
  xAxisLabels: number[] = [];
  yAxisLabels: number[] = [];
  
  ngOnInit(): void {
    this.initializeGrid();
    this.initializeAxisLabels();
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
  
  private initializeAxisLabels(): void {
    // Criar marcações do eixo X (0, 5, 10, 15, 20, 25, 30, 35, 40)
    this.xAxisLabels = [];
    for (let i = 0; i <= 40; i += 5) {
      this.xAxisLabels.push(i);
    }
    
    // Criar marcações do eixo Y (0, 5, 10, 15, 20, 25) - de baixo para cima
    this.yAxisLabels = [];
    for (let i = 0; i <= 25; i += 5) {
      this.yAxisLabels.push(i);
    }
  }
  
  getXLabelPosition(index: number): number {
    // Calcular posição baseada no índice e largura da célula
    const position = (index * 5) * this.cellSize;
    return Math.max(0, position - 15); // Ajuste para centralizar o label
  }
  
  getYLabelPosition(index: number): number {
    // Calcular posição baseada no índice e altura da célula
    // Labels de baixo para cima: 0 na base, 25 no topo
    const position = (this.gridHeight - 1 - (index * 5)) * this.cellSize;
    return Math.max(0, position - 12); // Ajuste para centralizar o label
  }
  
  private updateDronePositions(): void {
    this.initializeGrid();
    
    // Marcar a base na posição (0,0) - canto esquerdo inferior
    const baseGridX = 0;
    const baseGridY = this.gridHeight - 1; // Última linha (parte inferior)
    this.grid[baseGridY][baseGridX] = {
      type: 'base',
      drone: null
    };
    
    this.drones.forEach(drone => {
      // Converter coordenadas para posições na grade
      // Agora (0,0) está no canto esquerdo inferior
      const currentGridX = Math.floor(drone.currentX);
      // Inverter o eixo Y: quanto maior o Y, menor a posição na grade (de baixo para cima)
      const currentGridY = this.gridHeight - 1 - Math.floor(drone.currentY);
      const targetGridX = Math.floor(drone.targetX);
      const targetGridY = this.gridHeight - 1 - Math.floor(drone.targetY);
      
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
      'Disponível': 'primary',
      'Em Uso': 'accent',
      'Manutenção': 'warn',
      'Offline': 'disabled'
    };
    return colors[status] || 'primary';
  }
  
  getDroneStatusLabel(status: string): string {
    const labels: { [key: string]: string } = {
      'Disponível': 'Disponível',
      'Em Uso': 'Em Uso',
      'Manutenção': 'Manutenção',
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