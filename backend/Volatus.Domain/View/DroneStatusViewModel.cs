namespace Volatus.Domain.View;

public class DroneStatusViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int BatteryLevel { get; set; }
    public DateTime LastUpdate { get; set; }
    
    // Position properties
    public double CurrentX { get; set; }
    public double CurrentY { get; set; }
    public double TargetX { get; set; }
    public double TargetY { get; set; }
} 