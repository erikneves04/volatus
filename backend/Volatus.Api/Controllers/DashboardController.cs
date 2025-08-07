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
public class DashboardController : BaseController
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
} 