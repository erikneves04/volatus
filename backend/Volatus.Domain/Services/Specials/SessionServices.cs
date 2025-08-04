using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Volatus.Domain.Interfaces.Repositories;

using Volatus.Domain.Interfaces.Services.Specials;

namespace Volatus.Domain.Services.Specials;

public class SessionServices : ISessionServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserPermissionRepository _userPermissionRepository;

    public Guid _userId { get; private set; }
    public List<string> _permissions { get; private set; }

    public SessionServices(IHttpContextAccessor httpContextAccessor, IUserPermissionRepository userPermissionRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userPermissionRepository = userPermissionRepository;

        _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst("userId")?.Value);
        _permissions = _userPermissionRepository.Query().Where(e => e.UserId == _userId).Select(e => e.Permission.Name).ToList();
    }

    public bool HasPermission(string permission)
    {
        return _permissions.Contains(permission);
    }

    public bool IsAuthenticated()
    {
        return _userId != Guid.Empty;
    }
}