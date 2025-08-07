using Volatus.Domain.Interfaces.Services;
using Volatus.Domain.Interfaces.Services.Specials;
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
[Authorize]
public class UserController : BaseController
{
    private readonly IUserServices _services;
    private readonly IAuthenticationServices _authenticationServices;

    public UserController(IUserServices services, IAuthenticationServices authenticationServices)
    {
        _services = services;
        _authenticationServices = authenticationServices;
    }

    [HttpGet]
    public ActionResult<Response<IEnumerable<UserViewModel>>> Get([FromQuery] PaginationParams @params)
    {
        var content = _services.View(@params);
        return BuildResponse(content, @params);
    }

    [HttpGet("{id}")]
    public ActionResult<Response<UserViewModel>> Get(Guid id)
    {
        var content = _services.View(id);
        return BuildResponse(content);
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult<Response<UserViewModel>> Post([FromBody] UserInsertViewModel model)
    {
        var content = _services.Insert(model);
        return BuildResponse(content, StatusCodeEnums.Created);
    }

    [HttpPut("{id}")]
    public ActionResult<Response<UserViewModel>> Put([FromBody] UserUpdateViewModel model, Guid id)
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

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<Response<UserTokenViewModel>> Login([FromBody] LoginParams login)
    {
        var content = _authenticationServices.Login(login);
        return BuildResponse(content);
    }
}