namespace Volatus.Domain.View;

public class DashboardMetricsViewModel
{
    public int TotalDeliveries { get; set; }
    public int CompletedDeliveries { get; set; }
    public double AverageDeliveryTimeSeconds { get; set; }
    public double SuccessRate { get; set; }
} 