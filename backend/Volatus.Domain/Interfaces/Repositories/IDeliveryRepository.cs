using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Repositories;

public interface IDeliveryRepository : IRepository<Delivery>
{
    /// <summary>
    /// Gets all pending deliveries
    /// </summary>
    /// <returns>List of pending deliveries</returns>
    Task<List<Delivery>> GetPendingDeliveriesAsync();
    
    /// <summary>
    /// Gets all deliveries assigned to a specific drone
    /// </summary>
    /// <param name="droneId">Drone ID</param>
    /// <returns>List of deliveries for the drone</returns>
    Task<List<Delivery>> GetDeliveriesByDroneIdAsync(Guid droneId);
} 