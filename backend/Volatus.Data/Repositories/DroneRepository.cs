using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Data.Context;

namespace Volatus.Data.Repositories;

public class DroneRepository : Repository<Drone>, IDroneRepository
{
    public DroneRepository(AppDbContext context) : base(context) { }
} 