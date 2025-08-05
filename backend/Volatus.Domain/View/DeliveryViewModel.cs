using Volatus.Domain.Entities;

namespace Volatus.Domain.View;

public abstract class DeliveryViewModelBase
{
    public DeliveryViewModelBase() { }

    public DeliveryViewModelBase(Delivery delivery)
    : this(delivery.CustomerName, delivery.CustomerAddress, delivery.Description, delivery.Weight, delivery.Status, delivery.DeliveredDate, delivery.Notes, delivery.DroneId)
    { }

    public DeliveryViewModelBase(string customerName, string customerAddress, string description, double weight, string status, DateTime? deliveredDate, string? notes, Guid? droneId)
    {
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        Description = description;
        Weight = weight;
        Status = status;
        DeliveredDate = deliveredDate;
        Notes = notes;
        DroneId = droneId;
    }

    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; }
    public string Status { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? Notes { get; set; }
    public Guid? DroneId { get; set; }
}

public class DeliveryViewModel : DeliveryViewModelBase
{
    public DeliveryViewModel() { }

    public DeliveryViewModel(Guid id, string customerName, string customerAddress, string description, double weight, string status, DateTime? deliveredDate, string? notes, Guid? droneId) 
        : base(customerName, customerAddress, description, weight, status, deliveredDate, notes, droneId)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public DeliveryViewModel(Delivery delivery) : base(delivery)
    {
        Id = delivery.Id;
        CreatedAt = delivery.CreatedAt;
        UpdatedAt = delivery.UpdatedAt;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DeliveryInsertViewModel : DeliveryViewModelBase
{ }

public class DeliveryUpdateViewModel : DeliveryViewModelBase
{ }

public class DeliveryAssignmentViewModel
{
    public Guid DeliveryId { get; set; }
    public Guid DroneId { get; set; }
} 