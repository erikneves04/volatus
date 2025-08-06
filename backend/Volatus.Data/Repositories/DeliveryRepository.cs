using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Repositories;
using Volatus.Data.Context;

namespace Volatus.Data.Repositories;

public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
{
    public DeliveryRepository(AppDbContext context) : base(context) { }
    
    public Task<List<Delivery>> GetPendingDeliveriesAsync()
    {
        var pendingDeliveries = Get(d => d.Status == "Pendente");
        return Task.FromResult(pendingDeliveries);
    }
    
    public Task<List<Delivery>> GetDeliveriesByDroneIdAsync(Guid droneId)
    {
        var deliveries = Get(d => d.DroneId == droneId);
        return Task.FromResult(deliveries);
    }
} 