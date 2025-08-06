using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volatus.Domain.Entities;

public class Drone : Entity
{
    public Drone() { }
    
    public Drone(string name, string model, string serialNumber, string status, double maxWeight, double batteryCapacity)
    {
        Name = name;
        Model = model;
        SerialNumber = serialNumber;
        Status = status;
        MaxWeight = maxWeight;
        BatteryCapacity = batteryCapacity;
        CurrentBattery = 100.0;
        CurrentX = 0;
        CurrentY = 0;
        TargetX = 0;
        TargetY = 0;
        Speed = 1.0; // units per time interval
        IsCharging = false;
    }

    public string Name { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; } // Disponível, Em Uso, Manutenção, Offline
    public double MaxWeight { get; set; } // kg
    public double BatteryCapacity { get; set; }
    public double CurrentBattery { get; set; } // percentage
    public string? Notes { get; set; }
    
    // Position and movement properties
    public double CurrentX { get; set; }
    public double CurrentY { get; set; } 
    public double TargetX { get; set; }
    public double TargetY { get; set; }
    public double Speed { get; set; } // units per time interval
    public bool IsCharging { get; set; }
    public DateTime? LastMovementTime { get; set; }
    

}

public class DroneConfiguration : IEntityTypeConfiguration<Drone>
{
    public void Configure(EntityTypeBuilder<Drone> builder)
    {
        builder.HasKey(drone => drone.Id);

        builder.Property(drone => drone.Name).HasMaxLength(150).IsRequired();
        builder.Property(drone => drone.Model).HasMaxLength(100).IsRequired();
        builder.Property(drone => drone.SerialNumber).HasMaxLength(100).IsRequired();
        builder.Property(drone => drone.Status).HasMaxLength(50).IsRequired();
        builder.Property(drone => drone.MaxWeight).IsRequired();
        builder.Property(drone => drone.BatteryCapacity).IsRequired();
        builder.Property(drone => drone.CurrentBattery).IsRequired();
        builder.Property(drone => drone.Notes).HasMaxLength(500);
        
        // Position and movement properties
        builder.Property(drone => drone.CurrentX).IsRequired();
        builder.Property(drone => drone.CurrentY).IsRequired();
        builder.Property(drone => drone.TargetX).IsRequired();
        builder.Property(drone => drone.TargetY).IsRequired();
        builder.Property(drone => drone.Speed).IsRequired();
        builder.Property(drone => drone.IsCharging).IsRequired();
        builder.Property(drone => drone.LastMovementTime);
        
        // Unique constraint for serial number
        builder.HasIndex(drone => drone.SerialNumber).IsUnique();
    }
} 