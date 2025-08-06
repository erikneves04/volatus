using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.View;
using Volatus.Api.Response;
using Volatus.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Volatus.Domain.Entities;
using System;

namespace Volatus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class DashboardController : Controller
{
    private readonly IDeliveryServices _deliveryServices;
    private readonly IDroneServices _droneServices;
    private readonly IEventServices _eventServices;

    public DashboardController(
        IDeliveryServices deliveryServices,
        IDroneServices droneServices,
        IEventServices eventServices)
    {
        _deliveryServices = deliveryServices;
        _droneServices = droneServices;
        _eventServices = eventServices;
    }

    [HttpGet("metrics")]
    public ActionResult<Response<DashboardMetricsViewModel>> GetMetrics()
    {
        var content = _deliveryServices.GetDashboardMetrics();
        return BuildResponse(content);
    }

    [HttpGet("drones/status")]
    public ActionResult<Response<IEnumerable<DroneStatusViewModel>>> GetDroneStatus()
    {
        var content = _droneServices.GetDroneStatus();
        return BuildResponse(content);
    }

    [HttpGet("deliveries/recent")]
    public ActionResult<Response<IEnumerable<DeliveryViewModel>>> GetRecentDeliveries([FromQuery] int count = 5)
    {
        var content = _deliveryServices.GetRecentDeliveries(count);
        return BuildResponse(content);
    }

    [HttpGet("events/recent")]
    public ActionResult<Response<IEnumerable<EventViewModel>>> GetRecentEvents([FromQuery] int count = 10)
    {
        var content = _eventServices.GetRecentEvents(count);
        return BuildResponse(content);
    }

    [HttpGet("drones/status/example")]
    public ActionResult<Response<IEnumerable<DroneStatusViewModel>>> GetDroneStatusExample()
    {
        // Dados de exemplo para testar o mapa
        var exampleDrones = new List<DroneStatusViewModel>
        {
            new DroneStatusViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Drone Alpha",
                SerialNumber = "DRN-001",
                Status = "Available",
                BatteryLevel = 85,
                LastUpdate = DateTime.UtcNow,
                CurrentX = -5.0,
                CurrentY = 3.0,
                TargetX = 8.0,
                TargetY = -2.0
            },
            new DroneStatusViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Drone Beta",
                SerialNumber = "DRN-002",
                Status = "InUse",
                BatteryLevel = 65,
                LastUpdate = DateTime.UtcNow,
                CurrentX = 2.0,
                CurrentY = -7.0,
                TargetX = -3.0,
                TargetY = 5.0
            },
            new DroneStatusViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Drone Gamma",
                SerialNumber = "DRN-003",
                Status = "Maintenance",
                BatteryLevel = 25,
                LastUpdate = DateTime.UtcNow,
                CurrentX = 0.0,
                CurrentY = 0.0,
                TargetX = 0.0,
                TargetY = 0.0
            },
            new DroneStatusViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Drone Delta",
                SerialNumber = "DRN-004",
                Status = "Available",
                BatteryLevel = 92,
                LastUpdate = DateTime.UtcNow,
                CurrentX = -8.0,
                CurrentY = -4.0,
                TargetX = 10.0,
                TargetY = 8.0
            },
            new DroneStatusViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Drone Epsilon",
                SerialNumber = "DRN-005",
                Status = "InUse",
                BatteryLevel = 45,
                LastUpdate = DateTime.UtcNow,
                CurrentX = 6.0,
                CurrentY = 1.0,
                TargetX = -6.0,
                TargetY = -1.0
            }
        };

        return BuildResponse(exampleDrones as IEnumerable<DroneStatusViewModel>);
    }
} 