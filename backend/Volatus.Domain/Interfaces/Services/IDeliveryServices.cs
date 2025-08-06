using Volatus.Domain.View;

namespace Volatus.Domain.Interfaces.Services;

public interface IDeliveryServices
{
    IEnumerable<DeliveryViewModel> View(PaginationParams @params);
    DeliveryViewModel View(Guid id);
    DeliveryViewModel Insert(DeliveryInsertViewModel model);
    DeliveryViewModel Update(DeliveryUpdateViewModel model, Guid id);
    void Delete(Guid id);
    DeliveryViewModel AssignDrone(DeliveryAssignmentViewModel model);
    DashboardMetricsViewModel GetDashboardMetrics();
    IEnumerable<DeliveryViewModel> GetRecentDeliveries(int count);
} 