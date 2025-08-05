using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volatus.Domain.Entities;

public class Delivery : Entity
{
    public Delivery() { }
    
    public Delivery(string customerName, string customerAddress, string description, double weight, string status, string priority)
    {
        CustomerName = customerName;
        CustomerAddress = customerAddress;
        Description = description;
        Weight = weight;
        Status = status;
        Priority = priority;
        ParseAddressToCoordinates();
    }

    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; } // Format: "(x, y)"
    public string Description { get; set; }
    public double Weight { get; set; } // in kg
    public string Status { get; set; } // Pending, InProgress, Delivered, Cancelled
    public string Priority { get; set; } // Low, Medium, High
    public DateTime? DeliveredDate { get; set; }
    public string? Notes { get; set; }
    public Guid? DroneId { get; set; } // Optional drone assignment
    public virtual Drone? Drone { get; set; }
    
    // Coordinate properties
    public double X { get; set; } // X coordinate
    public double Y { get; set; } // Y coordinate
    
    private void ParseAddressToCoordinates()
    {
        if (string.IsNullOrEmpty(CustomerAddress)) return;
        
        // Remove parentheses and split by comma
        var cleanAddress = CustomerAddress.Trim('(', ')');
        var parts = cleanAddress.Split(',');
        
        if (parts.Length == 2 && 
            double.TryParse(parts[0].Trim(), out double x) && 
            double.TryParse(parts[1].Trim(), out double y))
        {
            X = x;
            Y = y;
        }
    }
}

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.HasKey(delivery => delivery.Id);

        builder.Property(delivery => delivery.CustomerName).HasMaxLength(150).IsRequired();
        builder.Property(delivery => delivery.CustomerAddress).HasMaxLength(50).IsRequired();
        builder.Property(delivery => delivery.Description).HasMaxLength(500).IsRequired();
        builder.Property(delivery => delivery.Weight).IsRequired();
        builder.Property(delivery => delivery.Status).HasMaxLength(50).IsRequired();
        builder.Property(delivery => delivery.Priority).HasMaxLength(20).IsRequired();
        builder.Property(delivery => delivery.DeliveredDate);
        builder.Property(delivery => delivery.Notes).HasMaxLength(500);
        builder.Property(delivery => delivery.DroneId);
        
        // Coordinate properties
        builder.Property(delivery => delivery.X).IsRequired();
        builder.Property(delivery => delivery.Y).IsRequired();

        // Foreign key relationship with Drone (optional)
        builder.HasOne(delivery => delivery.Drone)
               .WithMany()
               .HasForeignKey(delivery => delivery.DroneId)
               .OnDelete(DeleteBehavior.SetNull);
    }
} 