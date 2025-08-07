namespace Volatus.Domain.View;

public class DashboardMetricsViewModel
{
    public int TotalDeliveries { get; set; }
    public int CompletedDeliveries { get; set; }
    public double AverageDeliveryTimeSeconds { get; set; }
    public double SuccessRate { get; set; }
    public MostEfficientDroneViewModel? MostEfficientDrone { get; set; }
}

public class MostEfficientDroneViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public int CompletedDeliveries { get; set; }
    public double EfficiencyRate { get; set; } // Percentage of successful deliveries
} 