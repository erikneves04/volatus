using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.View;
using Volatus.Api.Response;
using Volatus.Domain.Enums;

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Volatus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class EventController : Controller
{
    private readonly IEventServices _services;

    public EventController(IEventServices services)
    {
        _services = services;
    }

    [HttpGet]
    public ActionResult<Response<IEnumerable<EventViewModel>>> Get([FromQuery] int count = 50)
    {
        var content = _services.GetRecentEvents(count);
        return BuildResponse(content);
    }

    [HttpPost]
    public ActionResult<Response<EventViewModel>> Post([FromBody] CreateEventRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description))
        {
            return BuildResponse<EventViewModel>(null, StatusCodeEnums.BadRequest, new List<string> { "Título e descrição são obrigatórios" });
        }

        var content = _services.CreateEvent(request.Title, request.Description);
        return BuildResponse(content, StatusCodeEnums.Created);
    }
}

public class CreateEventRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
} 