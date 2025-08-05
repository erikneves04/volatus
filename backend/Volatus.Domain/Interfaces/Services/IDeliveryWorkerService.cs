using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Services;

public interface IDeliveryWorkerService
{
    /// <summary>
    /// Main worker method that allocates deliveries and updates drone positions
    /// </summary>
    /// <returns>Summary of operations performed</returns>
    Task<string> ProcessDeliverySystemAsync();
    
    /// <summary>
    /// Allocates pending deliveries to available drones
    /// </summary>
    /// <returns>Number of deliveries allocated</returns>
    int AllocateDeliveries();
    
    /// <summary>
    /// Updates drone positions and handles delivery completion
    /// </summary>
    /// <returns>Number of drones moved</returns>
    int UpdateDronePositions();
    
    /// <summary>
    /// Calculates optimal route for a drone to deliver multiple packages
    /// </summary>
    /// <param name="deliveries">List of deliveries to be delivered</param>
    /// <param name="startX">Starting X coordinate</param>
    /// <param name="startY">Starting Y coordinate</param>
    /// <returns>Ordered list of deliveries for optimal route</returns>
    List<Delivery> CalculateOptimalRoute(List<Delivery> deliveries, double startX, double startY);
    
    /// <summary>
    /// Calculates Euclidean distance between two points
    /// </summary>
    /// <param name="x1">First point X</param>
    /// <param name="y1">First point Y</param>
    /// <param name="x2">Second point X</param>
    /// <param name="y2">Second point Y</param>
    /// <returns>Distance between points</returns>
    double CalculateDistance(double x1, double y1, double x2, double y2);
    
    /// <summary>
    /// Checks if drone has sufficient battery for round trip
    /// </summary>
    /// <param name="drone">Drone to check</param>
    /// <param name="delivery">Delivery to check</param>
    /// <returns>True if drone has sufficient battery</returns>
    bool HasSufficientBatteryForDelivery(Drone drone, Delivery delivery);
} 