using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volatus.Domain.Interfaces.Services;

namespace Volatus.Api.Services;

public class DeliveryBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeliveryBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(3);

    public DeliveryBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<DeliveryBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Delivery Background Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var worker = scope.ServiceProvider.GetRequiredService<IDeliveryWorkerService>();
                    var result = await worker.ProcessDeliverySystemAsync();
                    _logger.LogInformation($"Delivery worker executed: {result}");
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Delivery Background Service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing delivery worker");
                // Don't re-throw the exception to prevent the service from stopping
            }

            try
            {
                await Task.Delay(_interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("Delivery Background Service stopped");
    }
} 