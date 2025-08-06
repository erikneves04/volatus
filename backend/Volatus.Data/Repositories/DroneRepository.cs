using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Data.Context;

namespace Volatus.Data.Repositories;

public class DroneRepository : Repository<Drone>, IDroneRepository
{
    public DroneRepository(AppDbContext context) : base(context) { }
    
    public Task<List<Drone>> GetAvailableDronesAsync()
    {
        var availableDrones = Get(d => d.Status == "Disponível");
        return Task.FromResult(availableDrones);
    }
    
    public Task<List<Drone>> GetActiveDronesAsync()
    {
        var activeDrones = Get(d => d.Status == "Disponível" || d.Status == "Em Uso" || d.Status == "Retornando à base");
        return Task.FromResult(activeDrones);
    }
} 