using Volatus.Domain.Entities;

namespace Volatus.Domain.View;

public abstract class DroneViewModelBase
{
    public DroneViewModelBase() { }

    public DroneViewModelBase(Drone drone)
    : this(drone.Name, drone.Model, drone.SerialNumber, drone.Status, drone.MaxWeight, drone.BatteryCapacity, drone.CurrentBattery, drone.Notes)
    { }

    public DroneViewModelBase(string name, string model, string serialNumber, string status, double maxWeight, double batteryCapacity, double currentBattery, string? notes)
    {
        Name = name;
        Model = model;
        SerialNumber = serialNumber;
        Status = status;
        MaxWeight = maxWeight;
        BatteryCapacity = batteryCapacity;
        CurrentBattery = currentBattery;
        Notes = notes;
    }

    public string Name { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; }
    public double MaxWeight { get; set; }
    public double BatteryCapacity { get; set; }
    public double CurrentBattery { get; set; }
    public string? Notes { get; set; }
}

public class DroneViewModel : DroneViewModelBase
{
    public DroneViewModel() { }

    public DroneViewModel(Guid id, string name, string model, string serialNumber, string status, double maxWeight, double batteryCapacity, double currentBattery, string? notes) 
        : base(name, model, serialNumber, status, maxWeight, batteryCapacity, currentBattery, notes)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public DroneViewModel(Drone drone) : base(drone)
    {
        Id = drone.Id;
        CreatedAt = drone.CreatedAt;
        UpdatedAt = drone.UpdatedAt;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DroneInsertViewModel : DroneViewModelBase
{ }

public class DroneUpdateViewModel : DroneViewModelBase
{ } 