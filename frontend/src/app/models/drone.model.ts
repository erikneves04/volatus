export interface Drone {
  id: string;
  name: string;
  model: string;
  serialNumber: string;
  status: string;
  maxWeight: number;
  batteryCapacity: number;
  currentBattery: number;
  notes?: string;
  createdAt?: string;
  updatedAt?: string;
}