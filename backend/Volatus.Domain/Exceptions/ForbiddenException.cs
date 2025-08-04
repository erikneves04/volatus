using Volatus.Domain.Enums;

namespace Volatus.Domain.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException()
    {
        StatusCode = StatusCodeEnums.Unauthorized;
    }

    public ForbiddenException(string message = "")
        : this()
    {
        Messages = new List<string>() { ConvertToReponseMessage(message) };
    }
    
    public ForbiddenException(List<string> messages)
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