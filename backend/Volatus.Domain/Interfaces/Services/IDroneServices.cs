using Volatus.Domain.View;

namespace Volatus.Domain.Interfaces.Services;

public interface IDroneServices
{
    IEnumerable<DroneViewModel> View(PaginationParams @params);
    DroneViewModel View(Guid id);
    DroneViewModel Insert(DroneInsertViewModel model);
    DroneViewModel Update(DroneUpdateViewModel model, Guid id);
    void Delete(Guid id);
} 