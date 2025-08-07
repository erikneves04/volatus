using Volatus.Domain.Entities;

namespace Volatus.Tests;

public static class TestHelpers
{
    public static class DeliveryBuilder
    {
        public static Delivery CreateDefaultDelivery()
        {
            return new Delivery("Test Customer", "(5,5)", "Test delivery", 1.0, "Pendente", "Média");
        }

        public static Delivery CreateDeliveryWithCoordinates(double x, double y)
        {
            return new Delivery("Test Customer", $"({x},{y})", "Test delivery", 1.0, "Pendente", "Média");
        }

        public static Delivery CreateDeliveryWithWeight(double weight)
        {
            return new Delivery("Test Customer", "(5,5)", "Test delivery", weight, "Pendente", "Média");
        }

        public static Delivery CreateDeliveryWithStatus(string status)
        {
            return new Delivery("Test Customer", "(5,5)", "Test delivery", 1.0, status, "Média");
        }

        public static Delivery CreateDeliveryWithPriority(string priority)
        {
            return new Delivery("Test Customer", "(5,5)", "Test delivery", 1.0, "Pendente", priority);
        }

        public static Delivery CreateHeavyDelivery()
        {
            return new Delivery("Test Customer", "(5,5)", "Heavy delivery", 20.0, "Pendente", "Média");
        }

        public static Delivery CreateLightDelivery()
        {
            return new Delivery("Test Customer", "(5,5)", "Light delivery", 0.1, "Pendente", "Média");
        }
    }

    public static class DroneBuilder
    {
        public static Drone CreateDefaultDrone()
        {
            return new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
        }

        public static Drone CreateDroneWithBattery(double battery)
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.CurrentBattery = battery;
            return drone;
        }

        public static Drone CreateDroneWithCapacity(double maxWeight)
        {
            return new Drone("Test Drone", "Model X", "SN001", "Disponível", maxWeight, 100.0);
        }

        public static Drone CreateDroneWithPosition(double x, double y)
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.CurrentX = x;
            drone.CurrentY = y;
            return drone;
        }

        public static Drone CreateDroneWithTarget(double targetX, double targetY)
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.TargetX = targetX;
            drone.TargetY = targetY;
            return drone;
        }

        public static Drone CreateDroneWithSpeed(double speed)
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.Speed = speed;
            return drone;
        }

        public static Drone CreateLowBatteryDrone()
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.CurrentBattery = 10.0;
            return drone;
        }

        public static Drone CreateFullBatteryDrone()
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.CurrentBattery = 100.0;
            return drone;
        }

        public static Drone CreateChargingDrone()
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Disponível", 10.0, 100.0);
            drone.IsCharging = true;
            drone.CurrentX = 0;
            drone.CurrentY = 0;
            return drone;
        }

        public static Drone CreateActiveDrone()
        {
            var drone = new Drone("Test Drone", "Model X", "SN001", "Em Uso", 10.0, 100.0);
            drone.CurrentX = 0;
            drone.CurrentY = 0;
            drone.TargetX = 10;
            drone.TargetY = 10;
            return drone;
        }
    }

    public static class TestData
    {
        public static List<Delivery> CreateSampleDeliveries()
        {
            return new List<Delivery>
            {
                new Delivery("Customer 1", "(1,1)", "Delivery 1", 1.0, "Pendente", "Alta"),
                new Delivery("Customer 2", "(5,5)", "Delivery 2", 2.0, "Pendente", "Média"),
                new Delivery("Customer 3", "(10,10)", "Delivery 3", 3.0, "Pendente", "Baixa"),
                new Delivery("Customer 4", "(15,15)", "Delivery 4", 4.0, "Em Progresso", "Média"),
                new Delivery("Customer 5", "(20,20)", "Delivery 5", 5.0, "Entregue", "Baixa")
            };
        }

        public static List<Drone> CreateSampleDrones()
        {
            return new List<Drone>
            {
                new Drone("Drone Alpha", "Model X", "SN001", "Disponível", 10.0, 100.0) { CurrentBattery = 100.0 },
                new Drone("Drone Beta", "Model Y", "SN002", "Em Uso", 15.0, 100.0) { CurrentBattery = 75.0 },
                new Drone("Drone Gamma", "Model Z", "SN003", "Disponível", 20.0, 100.0) { CurrentBattery = 50.0 },
                new Drone("Drone Delta", "Model W", "SN004", "Manutenção", 5.0, 100.0) { CurrentBattery = 25.0 },
                new Drone("Drone Epsilon", "Model V", "SN005", "Offline", 25.0, 100.0) { CurrentBattery = 0.0 }
            };
        }

        public static List<Delivery> CreateDeliveriesByPriority()
        {
            return new List<Delivery>
            {
                new Delivery("High Priority 1", "(1,1)", "High priority delivery 1", 1.0, "Pendente", "Alta"),
                new Delivery("High Priority 2", "(2,2)", "High priority delivery 2", 1.0, "Pendente", "Alta"),
                new Delivery("Medium Priority 1", "(3,3)", "Medium priority delivery 1", 1.0, "Pendente", "Média"),
                new Delivery("Medium Priority 2", "(4,4)", "Medium priority delivery 2", 1.0, "Pendente", "Média"),
                new Delivery("Low Priority 1", "(5,5)", "Low priority delivery 1", 1.0, "Pendente", "Baixa"),
                new Delivery("Low Priority 2", "(6,6)", "Low priority delivery 2", 1.0, "Pendente", "Baixa")
            };
        }

        public static List<Drone> CreateDronesByBatteryLevel()
        {
            return new List<Drone>
            {
                new Drone("High Battery", "Model X", "SN001", "Disponível", 10.0, 100.0) { CurrentBattery = 100.0 },
                new Drone("Medium Battery", "Model Y", "SN002", "Disponível", 10.0, 100.0) { CurrentBattery = 75.0 },
                new Drone("Low Battery", "Model Z", "SN003", "Disponível", 10.0, 100.0) { CurrentBattery = 25.0 },
                new Drone("Critical Battery", "Model W", "SN004", "Disponível", 10.0, 100.0) { CurrentBattery = 5.0 }
            };
        }

        public static List<Drone> CreateDronesByCapacity()
        {
            return new List<Drone>
            {
                new Drone("Small Capacity", "Model X", "SN001", "Disponível", 5.0, 100.0) { CurrentBattery = 100.0 },
                new Drone("Medium Capacity", "Model Y", "SN002", "Disponível", 10.0, 100.0) { CurrentBattery = 100.0 },
                new Drone("Large Capacity", "Model Z", "SN003", "Disponível", 20.0, 100.0) { CurrentBattery = 100.0 },
                new Drone("Extra Large Capacity", "Model W", "SN004", "Disponível", 50.0, 100.0) { CurrentBattery = 100.0 }
            };
        }
    }
} 