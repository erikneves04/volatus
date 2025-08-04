using Volatus.Domain.Enums;

namespace Volatus.Domain.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException()
    {
        StatusCode = StatusCodeEnums.Unauthorized;
    }

    public UnauthorizedException(string message = "")
        : this()
    {
        Messages = new List<string>() { ConvertToReponseMessage(message) };
    }
    
    public UnauthorizedException(List<string> messages)
        : this()
    {
        Messages = ConvertManyToResponseMessage(messages);
    }

    protected override string ConvertToReponseMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
            return "Permission not found.";

        return $"Permission: {message} not found";
    }
}