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
    }

    public string Name { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; } // Available, InUse, Maintenance, Offline
    public double MaxWeight { get; set; } // in kg
    public double BatteryCapacity { get; set; } // in minutes
    public double CurrentBattery { get; set; } // in percentage
    public string? Notes { get; set; }
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

        // Unique constraint for serial number
        builder.HasIndex(drone => drone.SerialNumber).IsUnique();
    }
} 