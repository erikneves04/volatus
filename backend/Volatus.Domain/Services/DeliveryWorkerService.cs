using Microsoft.Extensions.Logging;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Interfaces.Services;

namespace Volatus.Domain.Services;

public class DeliveryWorkerService : IDeliveryWorkerService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IDroneRepository _droneRepository;
    private readonly ILogger<DeliveryWorkerService> _logger;

    public DeliveryWorkerService(
        IDeliveryRepository deliveryRepository,
        IDroneRepository droneRepository,
        ILogger<DeliveryWorkerService> logger)
    {
        _deliveryRepository = deliveryRepository;
        _droneRepository = droneRepository;
        _logger = logger;
    }

    public async Task<string> ProcessDeliverySystemAsync()
    {
        try
        {
            var allocatedCount = AllocateDeliveries();
            var movedCount = UpdateDronePositions();
            
            return $"Allocated {allocatedCount} deliveries, moved {movedCount} drones";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing delivery system");
            throw;
        }
    }

    public int AllocateDeliveries()
    {
        try
        {
            // Get all deliveries and drones
            var allDeliveries = _deliveryRepository.Get().ToList();
            var allDrones = _droneRepository.Get().ToList();
            
            var pendingDeliveries = allDeliveries.Where(d => d.Status == "Pendente").ToList();
            if (!pendingDeliveries.Any())
            {
                _logger.LogInformation("No pending deliveries to allocate");
                return 0;
            }

            var availableDrones = allDrones.Where(d => 
                d.Status == "Disponível" || 
                (d.Status == "Disponível" && d.IsCharging && d.CurrentBattery >= 30)
            ).ToList();
            
            if (!availableDrones.Any())
            {
                _logger.LogWarning("No available drones for delivery allocation");
                return 0;
            }

            int allocatedCount = 0;

            // Group deliveries by priority
            var highPriorityDeliveries = pendingDeliveries.Where(d => d.Priority == "Alta").ToList();
            var mediumPriorityDeliveries = pendingDeliveries.Where(d => d.Priority == "Média").ToList();
            var lowPriorityDeliveries = pendingDeliveries.Where(d => d.Priority == "Baixa").ToList();

            // Process deliveries by priority
            var prioritizedDeliveries = highPriorityDeliveries.Concat(mediumPriorityDeliveries).Concat(lowPriorityDeliveries).ToList();

            foreach (var delivery in prioritizedDeliveries)
            {
                var bestDrone = FindBestDroneForDelivery(delivery, availableDrones);
                if (bestDrone != null)
                {
                    // Assign delivery to drone
                    delivery.DroneId = bestDrone.Id;
                    delivery.Status = "Em Progresso";
                    
                    // Set drone target to delivery location
                    bestDrone.TargetX = delivery.X;
                    bestDrone.TargetY = delivery.Y;
                    bestDrone.Status = "Em Uso";
                    bestDrone.IsCharging = false;
                    
                    _deliveryRepository.Update(delivery);
                    _droneRepository.Update(bestDrone);
                    
                    allocatedCount++;
                    _logger.LogInformation($"Allocated delivery {delivery.Id} to drone {bestDrone.Id}");
                }
            }

            return allocatedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error allocating deliveries");
            throw;
        }
    }

    public int UpdateDronePositions()
    {
        try
        {
            var allDrones = _droneRepository.Get().ToList();
            var activeDrones = allDrones.Where(d => 
                d.Status == "Em Uso" || 
                d.Status == "Retornando à base" || 
                (d.Status == "Disponível")
            ).ToList();
            
            int movedCount = 0;

            foreach (var drone in activeDrones)
            {
                if (drone.Status == "Em Uso")
                {
                    var reachedTarget = MoveDroneTowardsTarget(drone);
                    if (reachedTarget)
                    {
                        HandleDroneReachedTarget(drone);
                    }
                    movedCount++;
                }
                else if (drone.Status == "Retornando à base")
                {
                    var reachedBase = MoveDroneTowardsTarget(drone);
                    if (reachedBase)
                    {
                        HandleDroneReturnedToBase(drone);
                    }
                    movedCount++;
                }
                else if (drone.Status == "Disponível" && drone.IsCharging && IsAtBase(drone))
                {
                    ChargeDroneAtBase(drone);
                    movedCount++;
                }
            }

            return movedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating drone positions");
            throw;
        }
    }

    public List<Delivery> CalculateOptimalRoute(List<Delivery> deliveries, double startX, double startY)
    {
        if (!deliveries.Any()) return new List<Delivery>();

        var route = new List<Delivery>();
        var remainingDeliveries = new List<Delivery>(deliveries);
        var currentX = startX;
        var currentY = startY;

        // Use nearest neighbor algorithm for simplicity
        while (remainingDeliveries.Any())
        {
            var nearestDelivery = FindNearestDelivery(remainingDeliveries, currentX, currentY);
            route.Add(nearestDelivery);
            remainingDeliveries.Remove(nearestDelivery);
            currentX = nearestDelivery.X;
            currentY = nearestDelivery.Y;
        }

        return route;
    }

    public double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    public bool HasSufficientBatteryForDelivery(Drone drone, Delivery delivery)
    {
        var distanceToDelivery = CalculateDistance(drone.CurrentX, drone.CurrentY, delivery.X, delivery.Y);
        var distanceBackToBase = CalculateDistance(delivery.X, delivery.Y, 0, 0);
        var totalDistance = distanceToDelivery + distanceBackToBase;
        
        // Estimate battery consumption: 1% per unit distance
        var estimatedBatteryConsumption = totalDistance * 1.0;
        
        return drone.CurrentBattery >= estimatedBatteryConsumption;
    }

    private Drone? FindBestDroneForDelivery(Delivery delivery, List<Drone> availableDrones)
    {
        var suitableDrones = availableDrones.Where(d => 
            d.MaxWeight >= delivery.Weight &&
            HasSufficientBatteryForDelivery(d, delivery)
        ).ToList();

        if (!suitableDrones.Any()) return null;

        // Find drone with best battery level and capacity
        return suitableDrones.OrderByDescending(d => d.CurrentBattery)
                           .ThenByDescending(d => d.MaxWeight)
                           .FirstOrDefault();
    }

    private bool MoveDroneTowardsTarget(Drone drone)
    {
        var distanceToTarget = CalculateDistance(drone.CurrentX, drone.CurrentY, drone.TargetX, drone.TargetY);
        
        if (distanceToTarget <= drone.Speed)
        {
            // Reached target
            drone.CurrentX = drone.TargetX;
            drone.CurrentY = drone.TargetY;
            drone.LastMovementTime = DateTime.UtcNow;
            _droneRepository.Update(drone);
            return true;
        }
        else
        {
            // Move towards target
            var directionX = (drone.TargetX - drone.CurrentX) / distanceToTarget;
            var directionY = (drone.TargetY - drone.CurrentY) / distanceToTarget;
            
            drone.CurrentX += directionX * drone.Speed;
            drone.CurrentY += directionY * drone.Speed;
            drone.LastMovementTime = DateTime.UtcNow;
            
            // Consume battery
            var batteryConsumption = CalculateBatteryConsumption(drone.Speed);
            drone.CurrentBattery = Math.Max(0, drone.CurrentBattery - batteryConsumption);
            
            _droneRepository.Update(drone);
            return false;
        }
    }

    private void HandleDroneReachedTarget(Drone drone)
    {
        // Check if drone is at a delivery location
        var allDeliveries = _deliveryRepository.Get().ToList();
        var currentDelivery = allDeliveries.FirstOrDefault(d => 
            d.DroneId == drone.Id && d.Status == "Em Progresso");
        
        if (currentDelivery != null && IsAtDeliveryLocation(drone, currentDelivery))
        {
            // Complete delivery
            currentDelivery.Status = "Entregue";
            currentDelivery.DeliveredDate = DateTime.UtcNow;
            _deliveryRepository.Update(currentDelivery);
            
            // Set drone to return to base
            drone.TargetX = 0;
            drone.TargetY = 0;
            drone.Status = "Retornando à base";
            _droneRepository.Update(drone);
            
            _logger.LogInformation($"Delivery {currentDelivery.Id} completed by drone {drone.Id}");
        }
    }

    private void HandleDroneReturnedToBase(Drone drone)
    {
        if (IsAtBase(drone))
        {
            // Drone returned to base
            drone.Status = "Disponível";
            drone.IsCharging = true;
            _droneRepository.Update(drone);
            
            _logger.LogInformation($"Drone {drone.Id} returned to base and is now available");
        }
    }

    private void ChargeDroneAtBase(Drone drone)
    {
        if (IsAtBase(drone) && drone.CurrentBattery < 100)
        {
            drone.IsCharging = true;
            drone.CurrentBattery = Math.Min(100, drone.CurrentBattery + 5); // 5% per interval
            drone.LastMovementTime = DateTime.UtcNow;
            _droneRepository.Update(drone);
            
            _logger.LogInformation($"Drone {drone.Id} charging at base. Battery: {drone.CurrentBattery:F1}%");
        }
        else if (drone.CurrentBattery >= 100)
        {
            drone.IsCharging = false;
            _droneRepository.Update(drone);
        }
    }

    private bool IsAtBase(Drone drone)
    {
        return Math.Abs(drone.CurrentX) < 0.5 && Math.Abs(drone.CurrentY) < 0.5;
    }

    private bool IsAtDeliveryLocation(Drone drone, Delivery delivery)
    {
        var distance = CalculateDistance(drone.CurrentX, drone.CurrentY, delivery.X, delivery.Y);
        return distance <= 0.5;
    }

    private Delivery FindNearestDelivery(List<Delivery> deliveries, double currentX, double currentY)
    {
        return deliveries.OrderBy(d => CalculateDistance(currentX, currentY, d.X, d.Y)).First();
    }

    private double CalculateBatteryConsumption(double distance)
    {
        // Simple battery consumption model: 1% per unit distance
        return distance * 1.0;
    }
} 