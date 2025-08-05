export interface Delivery {
  id: string;
  customerName: string;
  customerAddress: string;
  customerPhone: string;
  description: string;
  weight: number;
  status: string;
  scheduledDate?: string;
  deliveredDate?: string;
  notes?: string;
  droneId?: string;
  createdAt?: string;
  updatedAt?: string;
} 