using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Services;

public interface IDeliveryAllocationService
{
    /// <summary>
    /// Allocates pending deliveries to available drones using optimal routing
    /// </summary>
    /// <returns>Number of deliveries allocated</returns>
    Task<int> AllocateDeliveriesAsync();
    
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
} 