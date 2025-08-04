using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.View;
using Volatus.Api.Response;
using Volatus.Domain.Enums;

using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Volatus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize("PermissionManager")]
public class PermissionController : Controller
{
    private readonly IPermissionServices _services;

    public PermissionController(IPermissionServices services)
    {
        _services = services;
    }

    [HttpGet]
    public ActionResult<Response<IEnumerable<PermissionViewModel>>> Get([FromQuery] PaginationParams @params)
    {
        var content = _services.View(@params);
        return BuildResponse(content, @params);
    }

    [HttpGet("{id}")]
    public ActionResult<Response<PermissionViewModel>> Get(Guid id)
    {
        var content = _services.View(id);
        return BuildResponse(content);
    }

    [HttpPost]
    public ActionResult<Response<PermissionViewModel>> Post([FromBody] PermissonCreateUpdateViewModel model)
    {
        var content = _services.Insert(model);
        return BuildResponse(content, StatusCodeEnums.Created);
    }

    [HttpPut("{id}")]
    public ActionResult<Response<PermissionViewModel>> Put([FromBody] PermissonCreateUpdateViewModel model, Guid id)
    {
        var content = _services.Update(model, id);
        return BuildResponse(content);
    }

    [HttpDelete("{id}")]
    public ActionResult<Response<NoContent>> Delete(Guid id)
    {
        _services.Delete(id);
        return BuildResponse();
    }
}