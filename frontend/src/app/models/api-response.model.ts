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