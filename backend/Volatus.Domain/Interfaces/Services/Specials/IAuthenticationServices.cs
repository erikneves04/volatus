using Volatus.Domain.View;

namespace Volatus.Domain.Interfaces.Services.Specials;

public interface IAuthenticationServices
{
    UserTokenViewModel Login(LoginParams login);
}