export interface Delivery {
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
  createdAt?: string;
  updatedAt?: string;
} 