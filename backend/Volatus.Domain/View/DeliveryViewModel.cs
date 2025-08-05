using Volatus.Domain.Entities;

namespace Volatus.Domain.View;

public abstract class DeliveryViewModelBase
{
    public DeliveryViewModelBase() { }

    public DeliveryViewModelBase(Delivery delivery)
    : this(delivery.CustomerName, delivery.CustomerAddress, delivery.CustomerPhone, delivery.Description, delivery.Weight, delivery.Status, delivery.ScheduledDate, delivery.DeliveredDate, delivery.Notes, delivery.DroneId)
    { }

    public DeliveryViewModelBase(string customerName, string customerAddress, string customerPhone, string description, double weight, string status, DateTime? scheduledDate, DateTime? deliveredDate, string? notes, Guid? droneId)
    {
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerPhone = customerPhone;
        Description = description;
        Weight = weight;
        Status = status;
        ScheduledDate = scheduledDate;
        DeliveredDate = deliveredDate;
        Notes = notes;
        DroneId = droneId;
    }

    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerPhone { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; }
    public string Status { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? Notes { get; set; }
    public Guid? DroneId { get; set; }
}

public class DeliveryViewModel : DeliveryViewModelBase
{
    public DeliveryViewModel() { }

    public DeliveryViewModel(Guid id, string customerName, string customerAddress, string customerPhone, string description, double weight, string status, DateTime? scheduledDate, DateTime? deliveredDate, string? notes, Guid? droneId) 
        : base(customerName, customerAddress, customerPhone, description, weight, status, scheduledDate, deliveredDate, notes, droneId)
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