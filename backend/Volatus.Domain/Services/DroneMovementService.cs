using Microsoft.Extensions.Logging;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Interfaces.Services;

namespace Volatus.Domain.Services;

public class DroneMovementService : IDroneMovementService
{
    private readonly IDroneRepository _droneRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ILogger<DroneMovementService> _logger;

    public DroneMovementService(
        IDroneRepository droneRepository,
        IDeliveryRepository deliveryRepository,
        ILogger<DroneMovementService> logger)
    {
        _droneRepository = droneRepository;
        _deliveryRepository = deliveryRepository;
        _logger = logger;
    }

    public async Task<int> UpdateDronePositionsAsync()
    {
        try
        {
            var activeDrones = await _droneRepository.GetActiveDronesAsync();
            int movedCount = 0;

            foreach (var drone in activeDrones)
            {
                if (drone.Status == "InUse")
                {
                    var reachedTarget = await MoveDroneTowardsTargetAsync(drone);
                    if (reachedTarget)
                    {
                        await HandleDroneReachedTargetAsync(drone);
                    }
                    movedCount++;
                }
                else if (drone.Status == "Available" && IsAtBase(drone))
                {
                    await ChargeDroneAtBaseAsync(drone);
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

    public async Task<bool> MoveDroneTowardsTargetAsync(Drone drone)
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

    public async Task ChargeDroneAtBaseAsync(Drone drone)
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

    public bool HasReachedTarget(Drone drone)
    {
        var distance = CalculateDistance(drone.CurrentX, drone.CurrentY, drone.TargetX, drone.TargetY);
        return distance <= 0.1; // Small threshold for reaching target
    }

    public double CalculateBatteryConsumption(double distance)
    {
        // Simple battery consumption model: 1% per unit distance
        return distance * 1.0;
    }

    private async Task HandleDroneReachedTargetAsync(Drone drone)
    {
        // Check if drone is at a delivery location
        var deliveries = await _deliveryRepository.GetDeliveriesByDroneIdAsync(drone.Id);
        var currentDelivery = deliveries.FirstOrDefault(d => d.Status == "InProgress");
        
        if (currentDelivery != null && IsAtDeliveryLocation(drone, currentDelivery))
        {
            // Complete delivery
            currentDelivery.Status = "Delivered";
            currentDelivery.DeliveredDate = DateTime.UtcNow;
            _deliveryRepository.Update(currentDelivery);
            
            // Set drone to return to base
            drone.TargetX = 0;
            drone.TargetY = 0;
            drone.Status = "Returning";
            _droneRepository.Update(drone);
            
            _logger.LogInformation($"Delivery {currentDelivery.Id} completed by drone {drone.Id}");
        }
        else if (IsAtBase(drone) && drone.Status == "Returning")
        {
            // Drone returned to base
            drone.Status = "Available";
            drone.IsCharging = true;
            _droneRepository.Update(drone);
            
            _logger.LogInformation($"Drone {drone.Id} returned to base and is now available");
        }
    }

    private bool IsAtBase(Drone drone)
    {
        return Math.Abs(drone.CurrentX) < 0.1 && Math.Abs(drone.CurrentY) < 0.1;
    }

    private bool IsAtDeliveryLocation(Drone drone, Delivery delivery)
    {
        var distance = CalculateDistance(drone.CurrentX, drone.CurrentY, delivery.X, delivery.Y);
        return distance <= 0.1;
    }

    private double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
} 