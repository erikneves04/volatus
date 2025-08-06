using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volatus.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volatus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkerController : ControllerBase
{
    private readonly IDeliveryWorkerService _deliveryWorkerService;
    private readonly ILogger<WorkerController> _logger;

    public WorkerController(
        IDeliveryWorkerService deliveryWorkerService,
        ILogger<WorkerController> logger)
    {
        _deliveryWorkerService = deliveryWorkerService;
        _logger = logger;
    }

    [HttpPost("process-deliveries")]
    public async Task<IActionResult> ProcessDeliveries()
    {
        try
        {
            var result = await _deliveryWorkerService.ProcessDeliverySystemAsync();
            return Ok(new { message = result, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing deliveries");
            return StatusCode(500, new { error = "Internal server error processing deliveries" });
        }
    }

    [HttpPost("allocate-deliveries")]
    public IActionResult AllocateDeliveries()
    {
        try
        {
            var allocatedCount = _deliveryWorkerService.AllocateDeliveries();
            return Ok(new { allocatedCount, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error allocating deliveries");
            return StatusCode(500, new { error = "Internal server error allocating deliveries" });
        }
    }

    [HttpPost("update-drone-positions")]
    public IActionResult UpdateDronePositions()
    {
        try
        {
            var movedCount = _deliveryWorkerService.UpdateDronePositions();
            return Ok(new { movedCount, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating drone positions");
            return StatusCode(500, new { error = "Internal server error updating drone positions" });
        }
    }

    [HttpPost("calculate-route")]
    public IActionResult CalculateRoute([FromBody] RouteCalculationRequest request)
    {
        try
        {
            // Convert request to Delivery objects for calculation
            var deliveries = request.Deliveries.Select(d => new Volatus.Domain.Entities.Delivery
            {
                Id = Guid.NewGuid(),
                X = d.X,
                Y = d.Y,
                Weight = d.Weight ?? 1.0
            }).ToList();

            var optimalRoute = _deliveryWorkerService.CalculateOptimalRoute(deliveries, request.StartX, request.StartY);
            
            var routeResult = optimalRoute.Select(d => new { X = d.X, Y = d.Y, Weight = d.Weight }).ToList();
            
            return Ok(new { route = routeResult, totalDistance = CalculateTotalDistance(optimalRoute, request.StartX, request.StartY) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating route");
            return StatusCode(500, new { error = "Internal server error calculating route" });
        }
    }

    private double CalculateTotalDistance(List<Volatus.Domain.Entities.Delivery> route, double startX, double startY)
    {
        if (!route.Any()) return 0;

        double totalDistance = 0;
        var currentX = startX;
        var currentY = startY;

        foreach (var delivery in route)
        {
            totalDistance += _deliveryWorkerService.CalculateDistance(currentX, currentY, delivery.X, delivery.Y);
            currentX = delivery.X;
            currentY = delivery.Y;
        }

        // Add distance back to base
        totalDistance += _deliveryWorkerService.CalculateDistance(currentX, currentY, 0, 0);

        return totalDistance;
    }
}

public class RouteCalculationRequest
{
    public List<DeliveryCoordinate> Deliveries { get; set; } = new();
    public double StartX { get; set; }
    public double StartY { get; set; }
}

public class DeliveryCoordinate
{
    public double X { get; set; }
    public double Y { get; set; }
    public double? Weight { get; set; }
} 