using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Services;

public interface IDroneMovementService
{
    /// <summary>
    /// Updates drone positions and handles delivery completion
    /// </summary>
    /// <returns>Number of drones moved</returns>
    Task<int> UpdateDronePositionsAsync();
    
    /// <summary>
    /// Moves a drone towards its target position
    /// </summary>
    /// <param name="drone">Drone to move</param>
    /// <returns>True if drone reached target, false otherwise</returns>
    Task<bool> MoveDroneTowardsTargetAsync(Drone drone);
    
    /// <summary>
    /// Handles drone charging when at base
    /// </summary>
    /// <param name="drone">Drone to charge</param>
    Task ChargeDroneAtBaseAsync(Drone drone);
    
    /// <summary>
    /// Checks if drone has reached its target
    /// </summary>
    /// <param name="drone">Drone to check</param>
    /// <returns>True if drone reached target</returns>
    bool HasReachedTarget(Drone drone);
    
    /// <summary>
    /// Calculates battery consumption for a distance
    /// </summary>
    /// <param name="distance">Distance traveled</param>
    /// <returns>Battery percentage consumed</returns>
    double CalculateBatteryConsumption(double distance);
} 