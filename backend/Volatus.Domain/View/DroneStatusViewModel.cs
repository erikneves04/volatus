namespace Volatus.Domain.View;

public class DroneStatusViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int BatteryLevel { get; set; }
    public DateTime LastUpdate { get; set; }
} 