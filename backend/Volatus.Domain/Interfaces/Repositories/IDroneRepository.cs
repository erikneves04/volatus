using Volatus.Domain.Entities;

namespace Volatus.Domain.Interfaces.Repositories;

public interface IDroneRepository : IRepository<Drone>
{
    /// <summary>
    /// Gets all available drones (Available status)
    /// </summary>
    /// <returns>List of available drones</returns>
    Task<List<Drone>> GetAvailableDronesAsync();
    
    /// <summary>
    /// Gets all active drones (Available, InUse, Returning status)
    /// </summary>
    /// <returns>List of active drones</returns>
    Task<List<Drone>> GetActiveDronesAsync();
} 