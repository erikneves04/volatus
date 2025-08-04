using System.Collections.Generic;


namespace Volatus.Domain.Interfaces.Services.Specials;

public interface ISessionServices
{
    bool HasPermission(string permission);
    bool IsAuthenticated();

    Guid _userId { get; }
    List<string> _permissions { get; }
}