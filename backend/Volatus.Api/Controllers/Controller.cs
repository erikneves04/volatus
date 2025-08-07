using Volatus.Api.Response;
using Volatus.Domain.Enums;
using Volatus.Domain.View;
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Volatus.Api.Controllers;

[ApiController]
[Route("api/base")]
public class BaseController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }

    [NonAction]
    protected ActionResult<Response<ObjectType>> BuildResponse<ObjectType>(ObjectType content, PaginationParams @params,int statusCode, List<string> messages = null)
        where ObjectType : class
    {
        return Response<ObjectType>.BuildResponse(statusCode, content, @params, messages);
    }

    [NonAction]
    protected ActionResult<Response<ObjectType>> BuildResponse<ObjectType>(
        ObjectType content, 
        PaginationParams @params = null, 
        StatusCodeEnums statusCode = StatusCodeEnums.Ok, 
        List<string> messages = null)
        where ObjectType : class
    {
        return Response<ObjectType>.BuildResponse(statusCode, content, @params, messages);
    }

    [NonAction]
    protected ActionResult<Response<ObjectType>> BuildResponse<ObjectType>(ObjectType content, StatusCodeEnums statusCode, List<string> messages = null)
        where ObjectType : class
    {
        return Response<ObjectType>.BuildResponse(statusCode, content, null, messages);
    }

    [NonAction]
    protected ActionResult<Response<NoContent>> BuildResponse(StatusCodeEnums statusCode = StatusCodeEnums.Ok, List<string> messages = null)
    {
        return Response<NoContent>.BuildResponse(statusCode, messages);
    }
}