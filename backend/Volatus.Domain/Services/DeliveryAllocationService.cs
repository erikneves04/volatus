using Microsoft.Extensions.Logging;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Domain.Interfaces.Services;

namespace Volatus.Domain.Services;

public class DeliveryAllocationService : IDeliveryAllocationService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IDroneRepository _droneRepository;
    private readonly ILogger<DeliveryAllocationService> _logger;

    public DeliveryAllocationService(
        IDeliveryRepository deliveryRepository,
        IDroneRepository droneRepository,
        ILogger<DeliveryAllocationService> logger)
    {
        _deliveryRepository = deliveryRepository;
        _droneRepository = droneRepository;
        _logger = logger;
    }

    public async Task<int> AllocateDeliveriesAsync()
    {
        try
        {
            // Get pending deliveries
            var pendingDeliveries = await _deliveryRepository.GetPendingDeliveriesAsync();
            if (!pendingDeliveries.Any())
            {
                _logger.LogInformation("No pending deliveries to allocate");
                return 0;
            }

            // Get available drones
            var availableDrones = await _droneRepository.GetAvailableDronesAsync();
            if (!availableDrones.Any())
            {
                _logger.LogWarning("No available drones for delivery allocation");
                return 0;
            }

            int allocatedCount = 0;

            // Group deliveries by priority
            var highPriorityDeliveries = pendingDeliveries.Where(d => d.Priority == "High").ToList();
            var mediumPriorityDeliveries = pendingDeliveries.Where(d => d.Priority == "Medium").ToList();
            var lowPriorityDeliveries = pendingDeliveries.Where(d => d.Priority == "Low").ToList();

            // Process deliveries by priority
            var allDeliveries = highPriorityDeliveries.Concat(mediumPriorityDeliveries).Concat(lowPriorityDeliveries).ToList();

            foreach (var delivery in allDeliveries)
            {
                var bestDrone = FindBestDroneForDelivery(delivery, availableDrones);
                if (bestDrone != null)
                {
                    // Assign delivery to drone
                    delivery.DroneId = bestDrone.Id;
                    delivery.Status = "InProgress";
                    
                    _deliveryRepository.Update(delivery);
                    
                    // Update drone status
                    bestDrone.Status = "InUse";
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

    private Drone? FindBestDroneForDelivery(Delivery delivery, List<Drone> availableDrones)
    {
        var suitableDrones = availableDrones.Where(d => 
            d.CurrentBattery > 20 && // Minimum battery threshold
            d.MaxWeight >= delivery.Weight &&
            d.Status == "Available"
        ).ToList();

        if (!suitableDrones.Any()) return null;

        // Find drone with best battery level and capacity
        return suitableDrones.OrderByDescending(d => d.CurrentBattery)
                           .ThenByDescending(d => d.MaxWeight)
                           .FirstOrDefault();
    }

    private Delivery FindNearestDelivery(List<Delivery> deliveries, double currentX, double currentY)
    {
        return deliveries.OrderBy(d => CalculateDistance(currentX, currentY, d.X, d.Y)).First();
    }
} 