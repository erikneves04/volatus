export interface PaginationParams {
  sortColumn: string;
  sortDirection: string;
  pageNumber: number;
  pageSize: number;
  count: number;
  isLastPage: boolean;
}

export interface ApiResponse<T> {
  statusCode: number;
  statusCodeDescription: string;
  messages: string[];
  data: T;
  pagination?: PaginationParams;
}

export interface DroneViewModel {
  id: string;
  name: string;
  model: string;
  serialNumber: string;
  status: string;
  maxWeight: number;
  batteryCapacity: number;
  currentBattery: number;
  notes?: string;
  createdAt: string;
  updatedAt: string;
}

export interface DroneInsertViewModel {
  name: string;
  model: string;
  serialNumber: string;
  status: string;
  maxWeight: number;
  batteryCapacity: number;
  currentBattery: number;
  notes?: string;
}

export interface DroneUpdateViewModel {
  name: string;
  model: string;
  serialNumber: string;
  status: string;
  maxWeight: number;
  batteryCapacity: number;
  currentBattery: number;
  notes?: string;
}

export interface EventViewModel {
  id: string;
  title: string;
  description: string;
  createdAt: string;
}

export interface DeliveryViewModel {
  id: string;
  customerName: string;
  customerAddress: string; // Format: "(x, y)"
  description: string;
  weight: number;
  status: string;
  priority: string; // Baixa, Média, Alta
  deliveredDate?: string;
  notes?: string;
  droneId?: string;
  createdAt: string;
  updatedAt: string;
}

export interface DeliveryInsertViewModel {
  customerName: string;
  customerAddress: string; // Format: "(x, y)"
  description: string;
  weight: number;
  status: string;
  priority: string; // Baixa, Média, Alta
  deliveredDate?: string;
  notes?: string;
  droneId?: string;
}

export interface DeliveryUpdateViewModel {
  customerName: string;
  customerAddress: string; // Format: "(x, y)"
  description: string;
  weight: number;
  status: string;
  priority: string; // Baixa, Média, Alta
  deliveredDate?: string;
  notes?: string;
  droneId?: string;
}

export interface DeliveryAssignmentViewModel {
  deliveryId: string;
  droneId: string;
} 