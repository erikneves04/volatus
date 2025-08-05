using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volatus.Domain.Entities;

public class Delivery : Entity
{
    public Delivery() { }
    
    public Delivery(string customerName, string customerAddress, string customerPhone, string description, double weight, string status)
    {
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        CustomerPhone = customerPhone;
        Description = description;
        Weight = weight;
        Status = status;
    }

    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerPhone { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; } // in kg
    public string Status { get; set; } // Pending, InProgress, Delivered, Cancelled
    public DateTime? ScheduledDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? Notes { get; set; }
    public Guid? DroneId { get; set; } // Optional drone assignment
    public virtual Drone? Drone { get; set; }
}

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(delivery => delivery.Id);

        builder.Property(delivery => delivery.CustomerName).HasMaxLength(150).IsRequired();
        builder.Property(delivery => delivery.CustomerAddress).HasMaxLength(500).IsRequired();
        builder.Property(delivery => delivery.CustomerPhone).HasMaxLength(20).IsRequired();
        builder.Property(delivery => delivery.Description).HasMaxLength(500).IsRequired();
        builder.Property(delivery => delivery.Weight).IsRequired();
        builder.Property(delivery => delivery.Status).HasMaxLength(50).IsRequired();
        builder.Property(delivery => delivery.ScheduledDate);
        builder.Property(delivery => delivery.DeliveredDate);
        builder.Property(delivery => delivery.Notes).HasMaxLength(500);
        builder.Property(delivery => delivery.DroneId);

        // Foreign key relationship with Drone (optional)
        builder.HasOne(delivery => delivery.Drone)
               .WithMany()
               .HasForeignKey(delivery => delivery.DroneId)
               .OnDelete(DeleteBehavior.SetNull);
    }
} 